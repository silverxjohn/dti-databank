using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.InflationRateViewModels;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Hosting;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/InflationRates")]
    public class InflationRatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public InflationRatesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/InflationRates/Monthly
        [HttpGet("Monthly")]
        public List<YearViewModel> GetInflationRates()
        {
            var inflationRates = _context.InflationRates;

            var rates = inflationRates.Where(rate => rate.Month != null).Select(rate => new YearViewModel
            {
                ID = rate.ID,
                YearId = rate.Year.ID,
                Name = rate.Year.Name,
                Rate = rate.Rate,
                Months = inflationRates.Where(monthRate => monthRate.Month != null).Where(monthRate => monthRate.Year.ID == rate.Year.ID).Select(monthRate => new MonthViewModel
                {
                    ID = monthRate.ID,
                    MonthId = monthRate.Month.ID,
                    YearId = monthRate.Year.ID,
                    YearName = monthRate.Year.Name,
                    Name = monthRate.Month.Name,
                    Rate = monthRate.Rate
                }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList()
            }).GroupBy(x => x.YearId).Select(z => z.First()).ToList();

            return rates;
    
        }

        // GET: api/InflationRates/Annual
        [HttpGet("Annual")]
        public List<YearViewModel> GetAnnualInflationRates()
        {
            var inflationRates = _context.InflationRates;

            var rates = inflationRates.Where(rate => rate.Month == null).Select(rate => new YearViewModel
            {
                ID = rate.ID,
                YearId = rate.Year.ID,
                Name = rate.Year.Name,
                Rate = rate.Rate,
            }).GroupBy(z => z.YearId).Select(y => y.First()).ToList();

            return rates;
        }

        // GET: api/InflationRates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInflationRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inflationRate = await _context.InflationRates.SingleOrDefaultAsync(m => m.ID == id);

            if (inflationRate == null)
            {
                return NotFound();
            }

            return Ok(inflationRate);
        }

        // PUT: api/InflationRates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInflationRate([FromRoute] int id, [FromBody] InflationRate inflationRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inflationRate.ID)
            {
                return BadRequest();
            }

            _context.Entry(inflationRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InflationRateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var rates = _context.InflationRates.Include(iR => iR.Year).Include(eR => eR.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                Rate = xc.Rate,
                Name = xc.Year.Name
            }).FirstOrDefault(xz => xz.ID == inflationRate.ID);

            return Ok(rates);
        }

        // POST: api/InflationRates
        [HttpPost]
        public async Task<IActionResult> PostInflationRate([FromBody] InflationRate inflationRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.InflationRates.Add(inflationRate);
            await _context.SaveChangesAsync();

            var createdInflationRate = _context.InflationRates.Include(eR => eR.Year).Include(e => e.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                Rate = xc.Rate,
                Name = xc.Year.Name
            }).FirstOrDefault(e => e.ID == inflationRate.ID);

            return Ok(createdInflationRate);
        }

        // DELETE: api/InflationRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInflationRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inflationRate = await _context.InflationRates.SingleOrDefaultAsync(m => m.ID == id);
            if (inflationRate == null)
            {
                return NotFound();
            }

            _context.InflationRates.Remove(inflationRate);
            await _context.SaveChangesAsync();

            return Ok(inflationRate);
        }

        private bool InflationRateExists(int id)
        {
            return _context.InflationRates.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/InflationRates/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet annualSheet = workbook.CreateSheet("Annual");
                IRow annualRow = annualSheet.CreateRow(0);
                IRow annualRowFields = annualSheet.CreateRow(2);
                ISheet monthlySheet = workbook.CreateSheet("Monthly");
                IRow monthlyRow = monthlySheet.CreateRow(0);
                IRow monthlyRowFields = monthlySheet.CreateRow(2);

                var inflationRates = _context.InflationRates.Include(iRate => iRate.Year).Include(iRate => iRate.Month);

                var annualInflationRates = inflationRates.Where(iRate => iRate.Month == null).GroupBy(iRate => iRate.YearId).Select(iRate => iRate.First());

                annualRow.CreateCell(0).SetCellValue("Statistics on Inflation Rate");
                annualRowFields.CreateCell(0).SetCellValue("Year");
                annualRowFields.CreateCell(1).SetCellValue("Inflation Rate");

                var i = 3;

                foreach (var annualInflationRate in annualInflationRates)
                {
                    annualRow = annualSheet.CreateRow(i);

                    annualRow.CreateCell(0).SetCellValue(Int32.Parse(annualInflationRate.Year.Name));
                    annualRow.CreateCell(1).SetCellValue(annualInflationRate.Rate);

                    i++;
                }

                monthlyRow.CreateCell(0).SetCellValue("Statistics on Inflation Rate");
                monthlyRowFields.CreateCell(0).SetCellValue("Year");
                monthlyRowFields.CreateCell(1).SetCellValue("Month");
                monthlyRowFields.CreateCell(2).SetCellValue("Inflation Rate");

                var monthlyInflationRates = inflationRates.Where(rate => rate.Month != null).Select(rate => new YearViewModel
                {
                    ID = rate.ID,
                    YearId = rate.Year.ID,
                    Name = rate.Year.Name,
                    Rate = rate.Rate,
                    Months = inflationRates.Where(monthRate => monthRate.Month != null).Where(monthRate => monthRate.Year.ID == rate.Year.ID).Select(monthRate => new MonthViewModel
                    {
                        ID = monthRate.ID,
                        MonthId = monthRate.Month.ID,
                        YearId = monthRate.Year.ID,
                        YearName = monthRate.Year.Name,
                        Name = monthRate.Month.Name,
                        Rate = monthRate.Rate
                    }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList()
                }).GroupBy(x => x.YearId).Select(z => z.First()).ToList();

                var b = 3;

                foreach (var monthlyInflationRate in monthlyInflationRates)
                {
                    foreach (var month in monthlyInflationRate.Months)
                    {
                        monthlyRow = monthlySheet.CreateRow(b);

                        monthlyRow.CreateCell(0).SetCellValue(Int32.Parse(month.YearName));
                        monthlyRow.CreateCell(1).SetCellValue(month.Name);
                        monthlyRow.CreateCell(2).SetCellValue(month.Rate);

                        b++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "INFLATION_RATES_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}