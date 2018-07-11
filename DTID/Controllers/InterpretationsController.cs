using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.InterpretationViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Interpretations")]
    public class InterpretationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnviroment;

        public InterpretationsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnviroment = hostingEnvironment;
        }

        // GET: api/Interpretations
        [HttpGet]
        public IEnumerable<Interpretation> GetInterpretations()
        {
            return _context.Interpretations;
        }

        // GET: api/Interpretations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInterpretation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var interpretation = _context.Interpretations.Where(i => i.CannedIndicatorId == id).Select(i => new InterpretationViewModel {
                ID = i.ID,
                CannedId = i.CannedIndicatorId,
                Message = i.Message,
                Attachments = _context.Attachments.Where(a => a.InterpretationId == i.ID).Select(a => new AttachmentViewModel {
                    ID = a.ID,
                    Filename = a.Filename,
                    Mime = a.Mime,
                    HashedName = a.HashedName,
                    Extension = a.Extension,
                    NewName = a.Newname
                }).ToList()
            }).First();

            if (interpretation == null)
            {
                return NotFound();
            }

            return Ok(interpretation);
        }

        [HttpPost("{id}/Update/Upload")]
        public async Task<IActionResult> UploadInterpretation([FromRoute] int id, [FromForm] InterpretationDataViewModel interpretationDataViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != interpretationDataViewModel.ID)
            {
                return BadRequest();
            }

            var interpretation = _context.Interpretations.Where(i => i.CannedIndicatorId == interpretationDataViewModel.CannedId).Where(i => i.ID == id).First();

            interpretation.Message = interpretationDataViewModel.Message;

            if (interpretationDataViewModel.File.Length > 0)
            {
                var splittedName = interpretationDataViewModel.File.FileName.Split(".");

                var attachment = new Attachment
                {
                    Filename = interpretationDataViewModel.File.FileName,
                    InterpretationId = interpretation.ID,
                    Mime = interpretationDataViewModel.File.ContentType,
                    Extension = splittedName[1]      
                };
                var path = Path.Combine(_hostingEnviroment.WebRootPath, "pdfs", attachment.Newname);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    interpretationDataViewModel.File.CopyTo(stream);
                    stream.Position = 0;
                    stream.Close();
                }
                _context.Attachments.Add(attachment);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterpretationExists(id))
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


        // PUT: api/Interpretations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterpretation([FromRoute] int id, [FromBody] Interpretation interpretation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != interpretation.ID)
            {
                return BadRequest();
            }

            _context.Entry(interpretation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterpretationExists(id))  
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

        // POST: api/Interpretations
        [HttpPost]
        public async Task<IActionResult> PostInterpretation([FromBody] Interpretation interpretation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Interpretations.Add(interpretation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInterpretation", new { id = interpretation.ID }, interpretation);
        }

        // POST: api/Interpretations
        [HttpPost("Create/Upload")]
        public async Task<IActionResult> PostInterpretationWithAttachment([FromForm] InterpretationDataViewModel interpretationDataViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var interpretation = new Interpretation
            {
                Message = interpretationDataViewModel.Message,
                CannedIndicatorId = interpretationDataViewModel.CannedId
            };

            _context.Interpretations.Add(interpretation);

            var splittedName = interpretationDataViewModel.File.FileName.Split(".");

            var attachment = new Attachment
            {
                Filename = interpretationDataViewModel.File.FileName,
                InterpretationId = interpretation.ID,
                Mime = interpretationDataViewModel.File.ContentType,
                Extension = splittedName[1]
            };

            _context.Attachments.Add(attachment);

            var path = Path.Combine(_hostingEnviroment.WebRootPath, "pdfs", attachment.Newname);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                interpretationDataViewModel.File.CopyTo(stream);
                stream.Position = 0;
                stream.Close();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Interpretations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInterpretation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var interpretation = await _context.Interpretations.SingleOrDefaultAsync(m => m.ID == id);
            if (interpretation == null)
            {
                return NotFound();
            }

            _context.Interpretations.Remove(interpretation);
            await _context.SaveChangesAsync();

            return Ok(interpretation);
        }

        private bool InterpretationExists(int id)
        {
            return _context.Interpretations.Any(e => e.ID == id);
        }
    }
}