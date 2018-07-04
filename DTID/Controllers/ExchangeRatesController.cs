using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.ExchangeRateViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/ExchangeRates")]
    public class ExchangeRatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExchangeRatesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/ExchangeRates/Monthly for month data
        [HttpGet("Monthly")]
        public IEnumerable<YearViewModel> GetExchangeRates()
        {
            var exchangeRates = _context.ExchangeRates.Include(ex => ex.Month).Include(ez => ez.Year);

            var rates = exchangeRates.Where(er => er.MonthID == null).Select(eRates => new YearViewModel
            {
                ID = eRates.ID,
                YearId = eRates.Year.ID,
                Name = eRates.Year.Name,
                Rate = eRates.Rate,
                Months = exchangeRates.Where(eRate => eRate.MonthID != null).Where(eRate => eRate.YearId == eRates.YearId).Select(mERate => new MonthViewModel
                {
                    ID = mERate.ID,
                    MonthId = mERate.Month.ID,
                    YearId = mERate.Year.ID,
                    YearName = mERate.Year.Name,
                    Name = mERate.Month.Name,
                    Rate = mERate.Rate
                }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList()
            }).GroupBy(c => c.YearId).Select(n => n.First()).ToList();

            return rates;
        }

        // GET: api/ExchangeRates/Annual for year data
        [HttpGet("Annual")]
        public IEnumerable<YearViewModel> GetExchangeRatesMonth()
        {
            var exchangeRates = _context.ExchangeRates;

            var rates = exchangeRates.Where(er => er.MonthID == null).Select(eRates => new YearViewModel
            {
                ID = eRates.ID,
                YearId = eRates.Year.ID,
                Name = eRates.Year.Name,
                Rate = eRates.Rate,
            }).ToList();

            return rates;
        }

        // GET: api/ExchangeRates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exchangeRate = await _context.ExchangeRates.SingleOrDefaultAsync(m => m.ID == id);

            if (exchangeRate == null)
            {
                return NotFound();
            }

            return Ok(exchangeRate);
        }

        // PUT: api/ExchangeRates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExchangeRate([FromRoute] int id, [FromBody] ExchangeRate exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exchangeRate.ID)
            {
                return BadRequest();
            }

            _context.Entry(exchangeRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExchangeRateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedExchangeRate = _context.ExchangeRates.Include(eR => eR.Year).Include(e => e.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                Rate = xc.Rate,
                Name = xc.Year.Name
            }).FirstOrDefault(e => e.ID == exchangeRate.ID);

            return Ok(updatedExchangeRate);
        }

        // POST: api/ExchangeRates
        [HttpPost]
        public async Task<IActionResult> PostExchangeRate([FromBody] ExchangeRate exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync();

            var createdExchangeRate = _context.ExchangeRates.Include(eR => eR.Year).Include(e => e.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                Rate = xc.Rate,
                Name = xc.Year.Name
            }).FirstOrDefault(e => e.ID == exchangeRate.ID);

            return Ok(createdExchangeRate);
        }

        // DELETE: api/ExchangeRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchangeRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exchangeRate = await _context.ExchangeRates.SingleOrDefaultAsync(m => m.ID == id);
            if (exchangeRate == null)
            {
                return NotFound();
            }

            _context.ExchangeRates.Remove(exchangeRate);
            await _context.SaveChangesAsync();

            return Ok(exchangeRate);
        }

        private bool ExchangeRateExists(int id)
        {
            return _context.ExchangeRates.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/ExchangeRates/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet monthSheet = workbook.CreateSheet("Monthly");
                IRow monthRowLabels = monthSheet.CreateRow(0);
                monthRowLabels.CreateCell(0).SetCellValue("Year");
                monthRowLabels.CreateCell(1).SetCellValue("Month");
                monthRowLabels.CreateCell(2).SetCellValue("Exchange Rate");
                ISheet annualSheet = workbook.CreateSheet("Annual");
                IRow annualRowLabels = annualSheet.CreateRow(0);
                annualRowLabels.CreateCell(0).SetCellValue("Year");
                annualRowLabels.CreateCell(1).SetCellValue("Exchange Rate");

                var exchangeRates = _context.ExchangeRates;

                var rates = exchangeRates.Where(er => er.MonthID == null).Select(eRates => new YearViewModel
                {
                    ID = eRates.ID,
                    YearId = eRates.Year.ID,
                    Name = eRates.Year.Name,
                    Rate = eRates.Rate,
                    Months = exchangeRates.Where(eRate => eRate.MonthID != null).Where(eRate => eRate.YearId == eRates.YearId).Select(mERate => new MonthViewModel
                    {
                        ID = mERate.ID,
                        MonthId = mERate.Month.ID,
                        YearId = mERate.Year.ID,
                        YearName = mERate.Year.Name,
                        Name = mERate.Month.Name,
                        Rate = mERate.Rate
                    }).GroupBy(c => c.MonthId).Select(n => n.First()).ToList()
                }).GroupBy(c => c.YearId).Select(n => n.First()).ToList();

                var i = 1;

                foreach (var rate in rates)
                {
                    IRow annualRow = annualSheet.CreateRow(i);

                    annualRow.CreateCell(0).SetCellValue(rate.Name);
                    annualRow.CreateCell(1).SetCellValue(rate.Rate);
                    
                    foreach (var month in rate.Months)
                    {
                        IRow monthRow = monthSheet.CreateRow(i);

                        monthRow.CreateCell(0).SetCellValue(month.YearName);
                        monthRow.CreateCell(1).SetCellValue(month.Name);
                        monthRow.CreateCell(2).SetCellValue(month.Rate);
                    }

                    i++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "EXCHANGE_RATE_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}