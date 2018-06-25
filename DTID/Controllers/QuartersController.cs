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
    [Route("api/Quarters")]
    public class QuartersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuartersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Quarters
        [HttpGet]
        public IEnumerable<Quarter> GetQuarters()
        {
            return _context.Quarters;
        }

        // GET: api/Quarters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuarter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quarter = await _context.Quarters.SingleOrDefaultAsync(m => m.ID == id);

            if (quarter == null)
            {
                return NotFound();
            }

            return Ok(quarter);
        }

        // PUT: api/Quarters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuarter([FromRoute] int id, [FromBody] Quarter quarter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quarter.ID)
            {
                return BadRequest();
            }

            _context.Entry(quarter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuarterExists(id))
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

        // POST: api/Quarters
        [HttpPost]
        public async Task<IActionResult> PostQuarter([FromBody] Quarter quarter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Quarters.Add(quarter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuarter", new { id = quarter.ID }, quarter);
        }

        // DELETE: api/Quarters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuarter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quarter = await _context.Quarters.SingleOrDefaultAsync(m => m.ID == id);
            if (quarter == null)
            {
                return NotFound();
            }

            _context.Quarters.Remove(quarter);
            await _context.SaveChangesAsync();

            return Ok(quarter);
        }

        private bool QuarterExists(int id)
        {
            return _context.Quarters.Any(e => e.ID == id);
        }
    }
}