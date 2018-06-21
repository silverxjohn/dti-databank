using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DTID.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/SourceFile")]
    public class SourceFileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SourceFileController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{id}/Original")]
        public IActionResult GetOriginal([FromRoute] int id)
        {
            var sourceFile = _context.SourceFiles.Where(s => s.Indicator.ID == id).SingleOrDefault();
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "excels");
            var fileProvider = new PhysicalFileProvider(filePath);
            var fileInfo = fileProvider.GetFileInfo(sourceFile.Name);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.ms-excel";

            return File(readStream, mimeType, sourceFile.OriginalName);
        }

        [HttpGet("{id}/Empty")]
        public IActionResult GetEmpty([FromRoute] int id)
        {
            return NoContent();
        }

        [HttpGet("{id}/Updated")]
        public IActionResult GetUpdated([FromRoute] int id)
        {
            return NoContent();
        }
    }
}