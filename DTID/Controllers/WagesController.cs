using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Hosting;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Wages")]
    public class WagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public WagesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Wages
        [HttpGet]
        public IEnumerable<Wage> GetWages()
        {
            return _context.Wages.Include(wage => wage.Year).GroupBy(wage => wage.YearID).Select(wage => wage.First());
        }

        // GET: api/Wages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wage = await _context.Wages.Include(w => w.Year).SingleOrDefaultAsync(m => m.ID == id);

            if (wage == null)
            {
                return NotFound();
            }

            return Ok(wage);
        }

        // PUT: api/Wages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWage([FromRoute] int id, [FromBody] Wage wage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wage.ID)
            {
                return BadRequest();
            }

            _context.Entry(wage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedWage = _context.Wages.Include(w => w.Year).SingleOrDefaultAsync(w => w.ID == wage.ID);

            return Ok(updatedWage);
        }

        // POST: api/Wages
        [HttpPost]
        public async Task<IActionResult> PostWage([FromBody] Wage wage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            _context.Wages.Add(wage);
            await _context.SaveChangesAsync();

            return Ok(_context.Wages.Include(w => w.Year).FirstOrDefault(e => e.ID == wage.ID));
        }

        // DELETE: api/Wages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wage = await _context.Wages.SingleOrDefaultAsync(m => m.ID == id);
            if (wage == null)
            {
                return NotFound();
            }

            _context.Wages.Remove(wage);
            await _context.SaveChangesAsync();

            return Ok(wage);
        }

        private bool WageExists(int id)
        {
            return _context.Wages.Any(e => e.ID == id);
        }

        [HttpGet("Download")] //api/Wages/Download
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"excels/demo.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Annual");
                IRow row = excelSheet.CreateRow(0);
                IRow rowYear = excelSheet.CreateRow(1);
                IRow rowLabels = excelSheet.CreateRow(3);

                var wages = _context.Wages.Include(wage => wage.Year).GroupBy(wage => wage.YearID).Select(wage => wage.First());

                row.CreateCell(0).SetCellValue("Statistics on Philippine Wage");
                rowYear.CreateCell(0).SetCellValue(wages.First().Year.Name + "-" + wages.Last().Year.Name);
                rowLabels.CreateCell(0).SetCellValue("Year");
                rowLabels.CreateCell(1).SetCellValue("Monthly Wage Average");

                var i = 4;

                foreach (var wage in wages)
                {
                    row = excelSheet.CreateRow(i);

                    row.CreateCell(0).SetCellValue(Int32.Parse(wage.Year.Name));
                    row.CreateCell(1).SetCellValue(wage.Wages);

                    i++;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var newFileName = "WAGES_" + DateTime.Now.ToString("MM-dd-yyyy_hh:mm_tt") + ".xlsx";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", newFileName);
        }
    }
}