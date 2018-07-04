using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.BalanceOfPaymentViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/BalanceOfPayments")]
    public class BalanceOfPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BalanceOfPaymentsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/BalanceOfPayments/Quarterly
        [HttpGet("Quarterly")]
        public List<QuarterViewModel> GetQuarterlyBalanceOfPayments()
        {
            var quarterBalanceOfPayments = GetQuarterData();

            return quarterBalanceOfPayments;
        }

        // GET: api/BalanceOfPayments/Quarterly
        [HttpGet("Monthly")]
        public List<MonthViewModel> GetMonthlyBalanceOfPayments()
        {
            var monthBalanceOfPayments = GetMonthData();

            return monthBalanceOfPayments;
        }

        // GET: api/BalanceOfPayments/Annual
        [HttpGet("Annual")]
        public List<YearViewModel> GetAnnualBalanceOfPayments()
        {
            var data = _context.BalanceOfPayments;

            var balanceOfPayments = data.Where(yearBops => yearBops.MonthID == null).Where(yearBops => yearBops.QuarterID == null).Select(yearBops => new YearViewModel
            {
                ID = yearBops.ID,
                YearId = yearBops.Year.ID,
                Name = yearBops.Year.Name,
                BalanceOfPayments = yearBops.BalanceOfPayments,
            }).GroupBy(e => e.YearId).Select(r => r.First()).ToList();

            return balanceOfPayments;
        }

        // GET: api/BalanceOfPayments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBalanceOfPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanceOfPayment = await _context.BalanceOfPayments.SingleOrDefaultAsync(m => m.ID == id);

            if (balanceOfPayment == null)
            {
                return NotFound();
            }

            return Ok(balanceOfPayment);
        }

        // PUT: api/BalanceOfPayments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBalanceOfPayment([FromRoute] int id, [FromBody] BalanceOfPayment balanceOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != balanceOfPayment.ID)
            {
                return BadRequest();
            }

            _context.Entry(balanceOfPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BalanceOfPaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedExchangeRate = _context.BalanceOfPayments.Include(eR => eR.Year).Include(e => e.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                BalanceOfPayments = xc.BalanceOfPayments,
                Name = xc.Year.Name
            }).FirstOrDefault(e => e.ID == id);

            return Ok(updatedExchangeRate);
        }

        // POST: api/BalanceOfPayments
        [HttpPost]
        public async Task<IActionResult> PostBalanceOfPayment([FromBody] BalanceOfPayment balanceOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BalanceOfPayments.Add(balanceOfPayment);
            await _context.SaveChangesAsync();

            var createdBalanceOfPayment = _context.BalanceOfPayments.Include(eR => eR.Year).Include(e => e.Month).Select(xc => new YearViewModel
            {
                ID = xc.ID,
                YearId = xc.Year.ID,
                BalanceOfPayments = xc.BalanceOfPayments,
                Name = xc.Year.Name
            }).FirstOrDefault(e => e.ID == balanceOfPayment.ID);

            return Ok(createdBalanceOfPayment);
        }

        // DELETE: api/BalanceOfPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBalanceOfPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanceOfPayment = await _context.BalanceOfPayments.SingleOrDefaultAsync(m => m.ID == id);
            if (balanceOfPayment == null)
            {
                return NotFound();
            }

            _context.BalanceOfPayments.Remove(balanceOfPayment);
            await _context.SaveChangesAsync();

            return Ok(balanceOfPayment);
        }

        private bool BalanceOfPaymentExists(int id)
        {
            return _context.BalanceOfPayments.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/BalanceOfPayments/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                // annual sheet
                ISheet annualSheet = workbook.CreateSheet("Annual");
                IRow annualHead = annualSheet.CreateRow(0);
                annualHead.CreateCell(0).SetCellValue("BALANCE OF PAYMENTS");
                IRow annualDescription = annualSheet.CreateRow(1);
                annualDescription.CreateCell(0).SetCellValue("in million US dollars");
                IRow annualRowLabels = annualSheet.CreateRow(3);
                annualRowLabels.CreateCell(0).SetCellValue("YEAR");
                annualRowLabels.CreateCell(1).SetCellValue("Overall BOP Position");
                // quarter sheet
                ISheet quarterSheet = workbook.CreateSheet("Quarterly");
                IRow quarterHead = quarterSheet.CreateRow(0);
                quarterHead.CreateCell(0).SetCellValue("BALANCE OF PAYMENTS");
                IRow quarterDescription = quarterSheet.CreateRow(1);
                quarterDescription.CreateCell(0).SetCellValue("in million US dollars");
                IRow quarterRowLabels = quarterSheet.CreateRow(3);
                quarterRowLabels.CreateCell(0).SetCellValue("YEAR");
                quarterRowLabels.CreateCell(1).SetCellValue("QTR");
                quarterRowLabels.CreateCell(2).SetCellValue("Overall BOP Position");
                //month sheet
                ISheet monthSheet = workbook.CreateSheet("Monthly");
                IRow monthHead = monthSheet.CreateRow(0);
                monthHead.CreateCell(0).SetCellValue("BALANCE OF PAYMENTS");
                IRow monthDescription = monthSheet.CreateRow(1);
                monthDescription.CreateCell(0).SetCellValue("in million US dollars");
                IRow monthRowLabels = monthSheet.CreateRow(3);
                monthRowLabels.CreateCell(0).SetCellValue("YEAR");
                monthRowLabels.CreateCell(1).SetCellValue("MONTH");
                monthRowLabels.CreateCell(2).SetCellValue("Overall BOP Position");

                var annualBops = GetAnnualData();
                var i = 4;

                foreach (var annualBop in annualBops)
                {
                    IRow yearRow = annualSheet.CreateRow(i);

                    yearRow.CreateCell(0).SetCellValue(Int32.Parse(annualBop.Name));
                    yearRow.CreateCell(1).SetCellValue(annualBop.BalanceOfPayments);

                    i++;
                }

                var quarterBops = GetQuarterData();
                var x = 4;

                foreach (var quarterBop in quarterBops)
                {
                    IRow quarterRow = quarterSheet.CreateRow(x);

                    quarterRow.CreateCell(0).SetCellValue(Int32.Parse(quarterBop.YearName));
                    quarterRow.CreateCell(1).SetCellValue(quarterBop.Name);
                    quarterRow.CreateCell(2).SetCellValue(quarterBop.BalanceOfPayments);

                    x++;
                }

                var monthBops = GetMonthData();
                var y = 4;

                foreach (var monthBop in monthBops)
                {
                    IRow monthRow = monthSheet.CreateRow(y);

                    monthRow.CreateCell(0).SetCellValue(Int32.Parse(monthBop.YearName));
                    monthRow.CreateCell(1).SetCellValue(monthBop.Name);
                    monthRow.CreateCell(2).SetCellValue(monthBop.BalanceOfPayments);

                    y++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "BOP_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }

        private List<YearViewModel> GetAnnualData()
        {
            var annualBalanceOfPayments = _context.BalanceOfPayments.Where(yearBops => yearBops.MonthID == null).Where(yearBops => yearBops.QuarterID == null).Select(yearBops => new YearViewModel
            {
                ID = yearBops.ID,
                YearId = yearBops.Year.ID,
                Name = yearBops.Year.Name,
                BalanceOfPayments = yearBops.BalanceOfPayments,
            }).OrderBy(z => z.YearId).GroupBy(xc => xc.YearId).Select(g => g.First()).ToList();

            return annualBalanceOfPayments;
        }

        private List<MonthViewModel> GetMonthData()
        {
            var monthBalanceOfPayments = _context.BalanceOfPayments.Where(monthBops => monthBops.QuarterID == null).Where(monthBops => monthBops.MonthID != null).Select(monthBops => new MonthViewModel
            {
                ID = monthBops.ID,
                YearName = monthBops.Year.Name,
                YearId = monthBops.Year.ID,
                MonthId = monthBops.Month.ID,
                Name = monthBops.Month.Name,
                BalanceOfPayments = monthBops.BalanceOfPayments
            }).OrderBy(z => z.YearId).GroupBy(mz => mz.MonthId).Select(z => z.First()).ToList();

            return monthBalanceOfPayments;
        }

        private List<QuarterViewModel> GetQuarterData()
        {
            var quarterBalanceOfPayments = _context.BalanceOfPayments.Where(quarterBops => quarterBops.MonthID == null).Where(quarterBops => quarterBops.QuarterID != null).Select(quarterBops => new QuarterViewModel
            {
                ID = quarterBops.ID,
                YearName = quarterBops.Year.Name,
                YearId = quarterBops.Year.ID,
                QuarterId = quarterBops.Quarter.ID,
                Name = quarterBops.Quarter.Name,
                BalanceOfPayments = quarterBops.BalanceOfPayments
            }).OrderBy(z => z.YearId).ToList();

            return quarterBalanceOfPayments;
        }
    }
}