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
        public IEnumerable<Indicator> GetIndicators()
        {
            return _context.Indicators.Where(indicator => indicator.IsActive);
        }

        // GET: api/Indicators/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIndicator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var indicator = await _context.Indicators.SingleOrDefaultAsync(m => m.ID == id);

            if (indicator == null)
            {
                return NotFound();
            }

            return Ok(indicator);
        }

        // GET: api/Indicators/all
        [HttpGet("all")]
        public IEnumerable<Indicator> GetAllIndicator()
        {
            return _context.Indicators;
        }

        // PUT: api/Indicators/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndicator([FromRoute] int id, [FromBody] PutIndicatorViewModel vm)
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

            #region "File Upload"
            //if (vm.File.Length > 0)
            //{
            //    var fileSplit = vm.File.FileName.Split(".");
            //    var fileExtension = fileSplit[fileSplit.Length - 1];

            //    var sourceFile = new SourceFile
            //    {
            //        OriginalName = vm.File.FileName,
            //        Indicator = indicator
            //    };
            //    sourceFile.Name = $"{sourceFile.Name}.{fileExtension}";
            //    indicator.Attachment = sourceFile;

            //    var path = Path.Combine(_hostingEnvironment.WebRootPath, "pdfs", sourceFile.Name);

            //    using (var stream = new FileStream(path, FileMode.Create))
            //    {
            //        vm.File.CopyTo(stream);
            //        stream.Position = 0;
            //    }
            //}
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