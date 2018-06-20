using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Years")]
    public class YearsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YearsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Years
        [HttpGet]
        public IEnumerable<Year> GetYears()
        {
            return _context.Years;
        }

        // GET: api/Years/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetYear([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var year = await _context.Years.SingleOrDefaultAsync(m => m.ID == id);

            if (year == null)
            {
                return NotFound();
            }

            return Ok(year);
        }

        // PUT: api/Years/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutYear([FromRoute] int id, [FromBody] Year year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != year.ID)
            {
                return BadRequest();
            }

            _context.Entry(year).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YearExists(id))
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

        // POST: api/Years
        [HttpPost]
        public async Task<IActionResult> PostYear([FromBody] Year year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Years.Add(year);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetYear", new { id = year.ID }, year);
        }

        // DELETE: api/Years/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteYear([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var year = await _context.Years.SingleOrDefaultAsync(m => m.ID == id);
            if (year == null)
            {
                return NotFound();
            }

            _context.Years.Remove(year);
            await _context.SaveChangesAsync();

            return Ok(year);
        }

        private bool YearExists(int id)
        {
            return _context.Years.Any(e => e.ID == id);
        }
    }
}