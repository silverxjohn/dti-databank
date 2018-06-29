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
    [Route("api/Wages")]
    public class WagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Wages
        [HttpGet]
        public IEnumerable<Wage> GetWages()
        {
            return _context.Wages.Include(wage => wage.Year).GroupBy(z => z.YearID).Select(r => r.First());
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
    }
}