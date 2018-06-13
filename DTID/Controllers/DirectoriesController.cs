using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTID.BusinessLogic.Models;
using DTID.BusinessLogic.ViewModels.DirectoryViewModels;
using DTID.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Directories")]
    public class DirectoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Directories
        [HttpGet]
        public IEnumerable<Object> GetFirstLevelDirectories()
        {
            return _context.Directories.Where(directory => directory.Parent == null);
        }

        [HttpGet("{id}")]
        public DirectoryViewModel GetDirectories([FromRoute] int id)
        {
            var vm = new DirectoryViewModel();

            vm.Directories = _context.Directories.Where(directory => directory.ParentID == id).ToList();
            vm.Indicators = _context.Indicators.Where(indicator => indicator.Parent.ID == id).ToList();

            return vm;
        }

        // PUT: api/ExchangeRates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirectory([FromRoute] int id, [FromBody] Directory directory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != directory.ID)
            {
                return BadRequest();
            }

            _context.Entry(directory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectoryExists(id))
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

        // POST: api/directories
        [HttpPost]
        public async Task<IActionResult> PostDirectory([FromBody] Directory directory)
        {
            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Directories.Add(directory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirectories", new { id = directory.ID }, directory);
        }

        // DELETE: api/ExchangeRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirectory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var directory = await _context.Directories.SingleOrDefaultAsync(m => m.ID == id);
            if (directory == null)
            {
                return NotFound();
            }

            _context.Directories.Remove(directory);
            await _context.SaveChangesAsync();

            return Ok(directory);
        }

        private bool DirectoryExists(int id)
        {
            return _context.Directories.Any(e => e.ID == id);
        }
    }
}