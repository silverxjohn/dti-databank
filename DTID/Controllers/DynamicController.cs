using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Dynamic")]
    public class DynamicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DynamicController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{id}")]
        public IActionResult GetIndicatorData([FromRoute] int id)
        {
            var indicator = _context.Indicators
                            .Include(i => i.Categories)
                            .Include("Categories.Columns")
                            .Include("Categories.Columns.Values")
                            .Where(i => i.ID == id).First();

            var data = new List<object>();
            
            foreach (var category in indicator.Categories)
            {
                var columnValues = category.Columns
                                            .SelectMany(column => column.Values)
                                            .OrderBy(column => column.RowId)
                                            .GroupBy(column => column.RowId)
                                            .Select(group => GetColumnData(group));

                data.Add(new Dictionary<string, object>
                {
                    { "sheet", GetCategoryData(category)},
                    { "columns", GetColumns(category) },
                    { "data", columnValues }
                });
            }

            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        private object GetColumns(Category category)
        {
            return category.Columns.Select(column => new
            {
                id = column.ID,
                name = column.Name,
                type = column.Type
            });
        }

        private object GetCategoryData(Category category)
        {
            return new
            {
                id = category.ID,
                name = category.Name,
                description = category.Description
            };
        }

        private Dictionary<string ,object> GetColumnData(IGrouping<int, ColumnValues> group)
        {
            var categoryData = new Dictionary<string, object>
            {
                { "id", group.First().RowId }
            };

            foreach (var value in group)
            {
                categoryData.Add(value.Column.Name, value.Value);
            }

            return categoryData;
        }

        [HttpPost("upload")]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file.Length <= 0)
            {
                return BadRequest();
            }

            var fileSplit = file.FileName.Split(".");
            var fileExtension = fileSplit[fileSplit.Length - 1];

            var sourceFile = new SourceFile
            {
                OriginalName = file.FileName
            };
            sourceFile.Name = $"{sourceFile.Name}.{fileExtension}";

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "excels", sourceFile.Name);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Position = 0;

                var fileName = String.Join(".", fileSplit.Skip(0).Take(fileSplit.Length - 1));

                if (fileName.Substring(0, 4) == "dti_")
                    fileName = fileName.Substring(4, fileName.Length - 4);

                var indicator = new Indicator
                {
                    Name = fileName,
                    File = sourceFile
                };

                foreach (var sheet in GetSheet(file, stream))
                {
                    var category = new Category
                    {
                        Name = sheet.SheetName,
                        Indicator = indicator
                    };

                    IRow headerRow = sheet.GetRow(0);

                    var columns = GetHeaderColumn(headerRow, sheet.GetRow(1));
                    var values = GetContents(sheet, columns, headerRow.LastCellNum);

                    category.Columns = columns;
                    
                    _context.Columns.AddRange(columns);
                    _context.ColumnValues.AddRange(values);
                    _context.Categories.Add(category);
                }

                _context.Indicators.Add(indicator);
            }

            _context.SaveChanges();

            return Ok();
        }

        private List<ColumnValues> GetContents(ISheet sheet, List<Column> columns, int rowLength)
        {
            var values = new List<ColumnValues>();
            var year = "";
            var quarter = "";
            var month = "";

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                for (var j = 0; j < columns.Count; j++)
                {
                    ICell cell = row.GetCell(j);
                    var column = columns[j];
                    var value = cell != null ? cell.ToString() : "";
                    
                    switch(column.Type)
                    {
                        case ColumnType.Year:
                            if (cell != null)
                                year = value;
                            value = year;
                            break;
                        case ColumnType.Quarter:
                            if (cell != null)
                                quarter = value;
                            value = quarter;
                            break;
                        case ColumnType.Month:
                            if (cell != null)
                                month = value;
                            value = month;
                            break;
                    }

                    values.Add(new ColumnValues
                    {
                        RowId = i,
                        Column = column,
                        Value = value
                    });
                }
            }

            return values;
        }

        private List<Column> GetHeaderColumn(IRow row, IRow typeRow)
        {
            var columns = new List<Column>();

            for (var i = 0; i < row.LastCellNum; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                var type = GetColumnType(cell, typeRow.GetCell(i));

                columns.Add(new Column
                {
                    Name = cell.ToString(),
                    Type = type
                });
            }

            return columns;
        }

        private ColumnType GetColumnType(ICell cell, ICell typeCell)
        {
            var value = "";
            if (cell.CellComment != null && cell.CellComment.String != null && !String.IsNullOrWhiteSpace(cell.CellComment.String.String))
                value = cell.CellComment.String.String;
            else
                value = typeCell.ToString();

            switch (value.ToLower())
            {
                case "numeric":
                    return ColumnType.Number;
                case "year":
                    return ColumnType.Year;
                case "quarter":
                    return ColumnType.Quarter;
                case "month":
                    return ColumnType.Month;
                case "text":
                default:
                    return ColumnType.Label;
            }
        }

        private List<ISheet> GetSheet(IFormFile file, FileStream stream)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var sheets = new List<ISheet>();

            if (extension == ".xls")
            {
                //This will read the Excel 97-2000 formats  
                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);

                for (var i = 0; i < hssfwb.NumberOfSheets; i++)
                {
                    sheets.Add(hssfwb.GetSheetAt(i));
                }
            }
            else
            {
                //This will read 2007 Excel format  
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);

                for (var i = 0; i < hssfwb.NumberOfSheets; i++)
                {
                    sheets.Add(hssfwb.GetSheetAt(i));
                }
            }

            return sheets;
        }
    }
}