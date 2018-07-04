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
    [Route("api/BoiPezas")]
    public class BoiPezasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BoiPezasController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/BoiPezas
        [HttpGet]
        public IEnumerable<BoiPeza> GetBoiPezas()
        {
            return _context.BoiPezas.Include(boipeza => boipeza.Year).GroupBy(boipeza => boipeza.YearId).Select(boipeza => boipeza.First());
        }

        // GET: api/BoiPezas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoiPeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boiPeza = await _context.BoiPezas.SingleOrDefaultAsync(m => m.ID == id);

            if (boiPeza == null)
            {
                return NotFound();
            }

            return Ok(boiPeza);
        }

        // PUT: api/BoiPezas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoiPeza([FromRoute] int id, [FromBody] BoiPeza boiPeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != boiPeza.ID)
            {
                return BadRequest();
            }

            _context.Entry(boiPeza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoiPezaExists(id))
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

        // POST: api/BoiPezas
        [HttpPost]
        public async Task<IActionResult> PostBoiPeza([FromBody] BoiPeza boiPeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BoiPezas.Add(boiPeza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoiPeza", new { id = boiPeza.ID }, boiPeza);
        }

        // DELETE: api/BoiPezas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoiPeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boiPeza = await _context.BoiPezas.SingleOrDefaultAsync(m => m.ID == id);
            if (boiPeza == null)
            {
                return NotFound();
            }

            _context.BoiPezas.Remove(boiPeza);
            await _context.SaveChangesAsync();

            return Ok(boiPeza);
        }

        private bool BoiPezaExists(int id)
        {
            return _context.BoiPezas.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/BoiPezas/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(System.IO.Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet boiSheet = workbook.CreateSheet("BOI");
                IRow boiHead = boiSheet.CreateRow(0);
                boiHead.CreateCell(0).SetCellValue("Statistics on BOI-Approved Investments");

                var boiPezas = _context.BoiPezas.Include(boiPeza => boiPeza.Year).GroupBy(boiPeza => boiPeza.YearId).Select(boiPeza => boiPeza.First());

                IRow boiYears = boiSheet.CreateRow(1);
                boiYears.CreateCell(0).SetCellValue(boiPezas.First().Year.Name + "-" + boiPezas.Last().Year.Name);

                IRow boiLabels = boiSheet.CreateRow(3);
                boiLabels.CreateCell(0).SetCellValue("Year");
                boiLabels.CreateCell(1).SetCellValue("Amount (in Billion PHP)");

                var i = 4;

                foreach (var boi in boiPezas)
                {
                    IRow boiRow = boiSheet.CreateRow(i);

                    boiRow.CreateCell(0).SetCellValue(Int32.Parse(boi.Year.Name));
                    boiRow.CreateCell(1).SetCellValue(boi.BOI);

                    i++;
                }

                ISheet pezaSheet = workbook.CreateSheet("PEZA");
                IRow pezaHead = pezaSheet.CreateRow(0);
                pezaHead.CreateCell(0).SetCellValue("Statistics on PEZA-Approved Investments");

                IRow pezaYears = pezaSheet.CreateRow(1);
                pezaYears.CreateCell(0).SetCellValue(boiPezas.First().Year.Name + "-" + boiPezas.Last().Year.Name);

                IRow pezaLabels = pezaSheet.CreateRow(3);
                pezaLabels.CreateCell(0).SetCellValue("Year");
                pezaLabels.CreateCell(1).SetCellValue("Amount (in Billion PHP)");

                var y = 4;

                foreach (var peza in boiPezas)
                {
                    IRow pezaRow = pezaSheet.CreateRow(y);

                    pezaRow.CreateCell(0).SetCellValue(Int32.Parse(peza.Year.Name));
                    pezaRow.CreateCell(1).SetCellValue(peza.Peza);

                    y++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "BOI_PEZA" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}