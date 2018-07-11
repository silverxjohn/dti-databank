using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Attachments")]
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AttachmentsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Attachments
        [HttpGet]
        public IEnumerable<Attachment> GetAttachments()
        {
            return _context.Attachments;
        }

        // GET: api/Attachments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.ID == id);

            if (attachment == null)
            {
                return NotFound();
            }

            return Ok(attachment);
        }

        [HttpGet("{id}/Download")]
        public async Task<IActionResult> Download([FromRoute] int id)
        {
            var attachment = _context.Attachments.Where(att => att.ID == id).First();
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"pdfs/" + attachment.Newname;

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            var memory = new MemoryStream();

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }   
            memory.Position = 0;

            return File(memory, attachment.Mime, attachment.Filename);
        }

        // PUT: api/Attachments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttachment([FromRoute] int id, [FromBody] Attachment attachment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attachment.ID)
            {
                return BadRequest();
            }

            _context.Entry(attachment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttachmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Attachments
        [HttpPost]
        public async Task<IActionResult> PostAttachment([FromBody] Attachment attachment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttachment", new { id = attachment.ID }, attachment);
        }

        // DELETE: api/Attachments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.ID == id);
            if (attachment == null)
            {
                return NotFound();
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return Ok(attachment);
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.ID == id);
        }
    }
}