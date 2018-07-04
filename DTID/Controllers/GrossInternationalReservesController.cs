using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.GrossInternationalReserveViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/GrossInternationalReserves")]
    public class GrossInternationalReservesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GrossInternationalReservesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/GrossInternationalReserves
        [HttpGet]
        public IEnumerable<YearViewModel> GetGrossInternationalReserves()
        {
            var girs = _context.GrossInternationalReserves;

            var rates = girs.Where(gir => gir.MonthID == null).Select(gir => new YearViewModel
            {
                ID = gir.ID,
                YearId = gir.YearID,
                Name = gir.Year.Name,
                Rate = gir.Rate,
                Months = girs.Where(mGir => mGir.MonthID != null).Where(mGir => mGir.YearID == gir.YearID).Select(mGir => new MonthViewModel
                {
                    ID = mGir.ID,
                    MonthId = mGir.Month.ID,
                    YearId = mGir.Year.ID,
                    YearName = mGir.Year.Name,
                    Name = mGir.Month.Name,
                    Rate = mGir.Rate
                }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList()
            }).GroupBy(c => c.YearId).Select(n => n.First()).ToList();

            return rates;
        }

        // GET: api/GrossInternationalReserves/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrossInternationalReserve([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grossInternationalReserve = await _context.GrossInternationalReserves.SingleOrDefaultAsync(m => m.ID == id);

            if (grossInternationalReserve == null)
            {
                return NotFound();
            }

            return Ok(grossInternationalReserve);
        }

        // PUT: api/GrossInternationalReserves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrossInternationalReserve([FromRoute] int id, [FromBody] GrossInternationalReserve grossInternationalReserve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grossInternationalReserve.ID)
            {
                return BadRequest();
            }

            _context.Entry(grossInternationalReserve).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrossInternationalReserveExists(id))
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

        // POST: api/GrossInternationalReserves
        [HttpPost]
        public async Task<IActionResult> PostGrossInternationalReserve([FromBody] GrossInternationalReserve grossInternationalReserve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GrossInternationalReserves.Add(grossInternationalReserve);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrossInternationalReserve", new { id = grossInternationalReserve.ID }, grossInternationalReserve);
        }

        // DELETE: api/GrossInternationalReserves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrossInternationalReserve([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grossInternationalReserve = await _context.GrossInternationalReserves.SingleOrDefaultAsync(m => m.ID == id);
            if (grossInternationalReserve == null)
            {
                return NotFound();
            }

            _context.GrossInternationalReserves.Remove(grossInternationalReserve);
            await _context.SaveChangesAsync();

            return Ok(grossInternationalReserve);
        }

        private bool GrossInternationalReserveExists(int id)
        {
            return _context.GrossInternationalReserves.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/GrossInternationalReserves/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Monthly");
                IRow rowFields = excelSheet.CreateRow(0);

                var girs = _context.GrossInternationalReserves;

                var monthlyGrossInternationalReserves = girs.Where(gir => gir.Month != null).Select(gir => new YearViewModel
                {
                    ID = gir.ID,
                    YearId = gir.YearID,
                    Name = gir.Year.Name,
                    Rate = gir.Rate,
                    Months = girs.Where(mGir => mGir.Month != null).Where(mGir => mGir.YearID == gir.YearID).Select(mGir => new MonthViewModel
                    {
                        ID = mGir.ID,
                        MonthId = mGir.Month.ID,
                        YearId = mGir.Year.ID,
                        YearName = mGir.Year.Name,
                        Name = mGir.Month.Name,
                        Rate = mGir.Rate
                    }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList().ToList()
                }).GroupBy(c => c.YearId).Select(n => n.First()).ToList();

                rowFields.CreateCell(0).SetCellValue("Year");
                rowFields.CreateCell(1).SetCellValue("Month");
                rowFields.CreateCell(2).SetCellValue("GIR");

                var i = 1;

                foreach (var monthlyGrossInternationalReserve in monthlyGrossInternationalReserves)
                {
                    foreach(var month in monthlyGrossInternationalReserve.Months)
                    {
                        IRow row = excelSheet.CreateRow(i);

                        row.CreateCell(0).SetCellValue(Int32.Parse(month.YearName));
                        row.CreateCell(1).SetCellValue(month.Name);
                        row.CreateCell(2).SetCellValue(month.Rate);

                        i++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "GIR_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}