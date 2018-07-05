using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DTID.BusinessLogic.Models;
using DTID.BusinessLogic.ViewModels.DynamicViewModels;
using DTID.Data;
using Microsoft.AspNetCore.Authorization;
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

        private Dictionary<string ,object> GetColumnData(IGrouping<int, ColumnValue> group)
        {
            var categoryData = new Dictionary<string, object>
            {
                { "id", group.First().RowId }
            };

            foreach (var value in group)
            {
                try
                {
                    categoryData.Add(value.Column.Name, value.Value);
                } catch(ArgumentException e)
                {

                }
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

            Indicator indicator = null;

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

                if (fileName.Length > 4) 
                    if (fileName.Substring(0, 4) == "dti_")
                        fileName = fileName.Substring(4, fileName.Length - 4);

                indicator = new Indicator
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

            indicator.File = null;
            indicator.Categories = null;

            return Ok(indicator);
        }

        private List<ColumnValue> GetContents(ISheet sheet, List<Column> columns, int rowLength)
        {
            var values = new List<ColumnValue>();
            var year = "";
            var quarter = "";
            var month = "";

            var startRow = sheet.FirstRowNum + 1;

            if (!IsUsingComment(sheet.GetRow(0).GetCell(0)))
                startRow += 1;

            for (var i = startRow; i <= sheet.LastRowNum; i++)
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

                    values.Add(new ColumnValue
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
            if (IsUsingComment(cell))
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

        private bool IsUsingComment(ICell firstRow)
        {
            return firstRow.CellComment != null && firstRow.CellComment.String != null && !String.IsNullOrWhiteSpace(firstRow.CellComment.String.String);
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

        [HttpDelete("{id}/Category/{categoryId}/Row/{rowId}")]
        public IActionResult DeleteData([FromRoute] int id, [FromRoute] int categoryId, [FromRoute] int rowId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var values = _context.ColumnValues.Where(value => value.Column.Category.Indicator.ID == id).Where(value => value.Column.Category.ID == categoryId).Where(value => value.RowId == rowId).ToList();

            _context.ColumnValues.RemoveRange(values);

            _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/Category/{categoryId}/Row/{rowId}")]
        public async Task<IActionResult> PutData([FromRoute] int id, [FromRoute] int categoryId, [FromRoute] int rowId, [FromBody] List<DynamicDataViewModel> rows)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var values = _context.ColumnValues.Where(value => value.Column.Category.Indicator.ID == id).Where(value => value.Column.Category.ID == categoryId).Where(value => value.RowId == rowId).ToList();

            foreach (var row in rows)
            {
                var columnValue = values.Single(value => value.ColumnID == row.RowId);
                columnValue.Value = row.Value;

                _context.Entry(columnValue).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/Row/{rowId}")]
        public async Task<IActionResult> CreateData([FromRoute] int id, [FromRoute] int rowId, [FromBody] List<DynamicDataViewModel> rows)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = rows.Select(value => new ColumnValue
            {
                ColumnID = value.RowId,
                Value = value.Value,
                RowId = rowId
            });

            _context.ColumnValues.AddRange(data);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}