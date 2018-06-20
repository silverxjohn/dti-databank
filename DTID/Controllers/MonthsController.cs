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
    [Route("api/Months")]
    public class MonthsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Months
        [HttpGet]
        public IEnumerable<Month> GetMonths()
        {
            return _context.Months;
        }

        // GET: api/Months/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonth([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var month = await _context.Months.SingleOrDefaultAsync(m => m.ID == id);

            if (month == null)
            {
                return NotFound();
            }

            return Ok(month);
        }

        // PUT: api/Months/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonth([FromRoute] int id, [FromBody] Month month)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != month.ID)
            {
                return BadRequest();
            }

            _context.Entry(month).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonthExists(id))
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

        // POST: api/Months
        [HttpPost]
        public async Task<IActionResult> PostMonth([FromBody] Month month)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Months.Add(month);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonth", new { id = month.ID }, month);
        }

        // DELETE: api/Months/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonth([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var month = await _context.Months.SingleOrDefaultAsync(m => m.ID == id);
            if (month == null)
            {
                return NotFound();
            }

            _context.Months.Remove(month);
            await _context.SaveChangesAsync();

            return Ok(month);
        }

        private bool MonthExists(int id)
        {
            return _context.Months.Any(e => e.ID == id);
        }
    }
}