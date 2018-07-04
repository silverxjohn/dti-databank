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
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Populations")]
    public class PopulationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PopulationsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Populations
        [HttpGet]
        public IEnumerable<Population> GetPopulations()
        {
            return _context.Populations.Include(population => population.Year).GroupBy(population => population.YearID).Select(population => population.First());
        }

        // GET: api/Populations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPopulation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var population = await _context.Populations.SingleOrDefaultAsync(m => m.ID == id);

            if (population == null)
            {
                return NotFound();
            }

            return Ok(population);
        }

        // PUT: api/Populations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPopulation([FromRoute] int id, [FromBody] Population population)
        {
            var populationToUpdate = _context.Populations.FirstOrDefault(populations => populations.ID == id);

            if (populationToUpdate == null)
            {
                return NotFound();
            }

            populationToUpdate.YearID = population.YearID;

            populationToUpdate.Populations = population.Populations;

            await _context.SaveChangesAsync();

            _context.Entry(population).State = EntityState.Modified;

            return Ok(populationToUpdate);
        }

        // POST: api/Populations
        [HttpPost]
        public async Task<IActionResult> PostPopulation([FromBody] Population population)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Populations.Add(population);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPopulation", new { id = population.ID }, population);
        }

        // DELETE: api/Populations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePopulation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var population = await _context.Populations.SingleOrDefaultAsync(m => m.ID == id);
            if (population == null)
            {
                return NotFound();
            }

            _context.Populations.Remove(population);
            await _context.SaveChangesAsync();

            return Ok(population);
        }

        private bool PopulationExists(int id)
        {
            return _context.Populations.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/Populations/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("popn");
                IRow rowLabels = excelSheet.CreateRow(0);

                var populations = _context.Populations.Include(popn => popn.Year).GroupBy(popn => popn.YearID).Select(popn => popn.First());

                rowLabels.CreateCell(1).SetCellValue("PH POPN");

                var i = 1;

                foreach (var population in populations)
                {
                    IRow row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(Int32.Parse(population.Year.Name));
                    row.CreateCell(1).SetCellValue(population.Populations);

                    i++;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "POPN_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}