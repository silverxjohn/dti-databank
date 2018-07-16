using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.Data;
using DTID.BusinessLogic.Models;
using DTID.BusinessLogic.ViewModels.IndicatorViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Indicators")]
    public class IndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IndicatorsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Indicators
        [HttpGet]
        public async Task<IActionResult> GetIndicators()
        {
            var indicator = _context.Indicators.Where(i => i.IsActive).Select(i => new ResponseViewModel
            {
                ID = i.ID,
                Name = i.Name,
                Description = i.Description,
                ParentID = i.ParentID,
                IsActive = i.IsActive,
                IsApproved = i.IsApproved,
                Attachments = _context.Attachments.Where(a => a.IndicatorId == i.ID).Select(a => new AttachmentViewModel
                {
                    ID = a.ID,
                    Filename = a.Filename,
                    Mime = a.Mime,
                    HashedName = a.HashedName,
                    Extension = a.Extension,
                    NewName = a.Newname
                }).ToList()
            });

            return Ok(indicator);
        }

        // GET: api/Indicators/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIndicator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var indicator = _context.Indicators.Where(i => i.ID == id).Select(i => new ResponseViewModel
            {
                ID = i.ID,
                Name = i.Name,
                Description = i.Description,
                ParentID = i.ParentID,
                IsActive = i.IsActive,
                IsApproved = i.IsApproved,
                Attachments = _context.Attachments.Where(a => a.IndicatorId == i.ID).Select(a => new AttachmentViewModel
                {
                    ID = a.ID,
                    Filename = a.Filename,
                    Mime = a.Mime,
                    HashedName = a.HashedName,
                    Extension = a.Extension,
                    NewName = a.Newname
                }).ToList()
            }).First();

            if (indicator == null)
            {
                return NotFound();
            }

            return Ok(indicator);
        }

        // GET: api/Indicators/all
        [HttpGet("all")]
        public IEnumerable<ResponseViewModel> GetAllIndicator()
        {
            var indicator = _context.Indicators.Select(i => new ResponseViewModel
            {
                ID = i.ID,
                Name = i.Name,
                Description = i.Description,
                ParentID = i.ParentID,
                IsActive = i.IsActive,
                IsApproved = i.IsApproved,
                Attachments = _context.Attachments.Where(a => a.IndicatorId == i.ID).Select(a => new AttachmentViewModel
                {
                    ID = a.ID,
                    Filename = a.Filename,
                    Mime = a.Mime,
                    HashedName = a.HashedName,
                    Extension = a.Extension,
                    NewName = a.Newname
                }).ToList()
            });

            return indicator;
        }

        // PUT: api/Indicators/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndicator([FromRoute] int id, [FromForm] PutIndicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vm.ID)
            {
                return BadRequest();
            }

            if (!IndicatorExists(id))
            {
                return NotFound();
            }

            var indicator = _context.Indicators.Find(id);
            indicator.Description = vm.Description;

            _context.SaveChanges();

            #region "File Upload"
            if (vm.File != null && vm.File.Length > 0)
            {
                var fileSplit = vm.File.FileName.Split(".");
                var fileExtension = fileSplit[fileSplit.Length - 1];

                var attachment = new Attachment
                {
                    Filename = vm.File.FileName,
                    Mime = vm.File.ContentType,
                    Extension = fileExtension,
                    IndicatorId = id
                };

                var path = Path.Combine(_hostingEnvironment.WebRootPath, "pdfs", attachment.Newname);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    vm.File.CopyTo(stream);
                    stream.Position = 0;
                }
                _context.Attachments.Add(attachment);
            }
            #endregion

            _context.Entry(indicator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicatorExists(id))
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

        // POST: api/Indicators
        [HttpPost]
        public async Task<IActionResult> PostIndicator([FromBody] Indicator indicator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Indicators.Add(indicator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndicator", new { id = indicator.ID }, indicator);
        }

        // DELETE: api/Indicators/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndicator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var indicator = await _context.Indicators
                                    .Include(i => i.Categories)
                                    .Include("Categories.Columns")
                                    .Include("Categories.Columns.Values")
                                    .SingleOrDefaultAsync(m => m.ID == id);

            if (indicator == null)
            {
                return NotFound();
            }

            _context.Indicators.Remove(indicator);

            try
            {
                await _context.SaveChangesAsync();

            } catch(Exception e)
            {

            }

            return Ok();
        }

        private bool IndicatorExists(int id)
        {
            return _context.Indicators.Any(e => e.ID == id);
        }
    }
}