using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DTID.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            //new DatabaseSeeder(context).Run();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("test")]
        public IActionResult GetWage()
        {
            var indicator = _context.Indicators.Include(i => i.Columns).FirstOrDefault();

            dynamic data = new ExpandoObject();

            foreach (var column in indicator.Columns)
            {
                var values = _context.ColumnValues.Where(val => val.Column.ID == column.ID).ToList();
                ((IDictionary<string, object>)data).Add(column.Name, values.Select(value => value.Value));
            }

            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        [HttpPost("upload")]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file.Length <= 0)
            {
                return BadRequest();
            }

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "excels", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Position = 0;

                var indicator = new Indicator
                {
                    Name = file.FileName
                };

                ISheet sheet = GetSheet(file, stream);

                IRow headerRow = sheet.GetRow(0);

                var columns = GetHeaderColumn(headerRow);
                var values = GetContents(sheet, columns, headerRow.LastCellNum);

                indicator.Columns = columns;
                
                

                _context.Columns.AddRange(columns);
                _context.ColumnValues.AddRange(values);
                _context.Indicators.Add(indicator);
            }

            _context.SaveChanges();

            return Ok();
        }

        public List<ColumnValues> GetContents(ISheet sheet, List<Column> columns, int rowLength)
        {
            var values = new List<ColumnValues>();

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                for (var j = row.FirstCellNum; j < rowLength; j++)
                {
                    ICell cell = row.GetCell(j);

                    if (cell == null) continue;

                    values.Add(new ColumnValues
                    {
                        RowId = i,
                        Column = columns[j],
                        Value = cell.ToString()
                    });
                }
            }

            return values;
        }

        public List<Column> GetHeaderColumn(IRow row)
        {
            var columns = new List<Column>();

            for (var i = 0; i < row.LastCellNum; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                ColumnType type = ColumnType.Label;

                switch(cell.CellComment.String.String)
                {
                    case "numeric":
                        type = ColumnType.Number;
                        break;
                    case "text":
                        type = ColumnType.Label;
                        break;
                }

                columns.Add(new Column
                {
                    Name = cell.ToString(),
                    Type = type
                });
            }

            return columns;
        }

        public ISheet GetSheet(IFormFile file, FileStream stream)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension == ".xls")
            {
                //This will read the Excel 97-2000 formats  
                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                return hssfwb.GetSheetAt(0);
            }
            else
            {
                //This will read 2007 Excel format  
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                return hssfwb.GetSheetAt(0);
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
