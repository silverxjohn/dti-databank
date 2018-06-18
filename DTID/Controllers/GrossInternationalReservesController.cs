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
    [Route("api/GrossInternationalReserves")]
    public class GrossInternationalReservesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrossInternationalReservesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GrossInternationalReserves
        [HttpGet]
        public IEnumerable<GrossInternationalReserve> GetGrossInternationalReserves()
        {
            return _context.GrossInternationalReserves.Include(gir => gir.Year);
        }

        // GET: api/GrossInternationalReserves/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrossInternationalReserve([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grossInternationalReserve = await _context.GrossInternationalReserves.SingleOrDefaultAsync(m => m.ID == id);

            if (grossInternationalReserve == null)
            {
                return NotFound();
            }

            return Ok(grossInternationalReserve);
        }

        // PUT: api/GrossInternationalReserves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrossInternationalReserve([FromRoute] int id, [FromBody] GrossInternationalReserve grossInternationalReserve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grossInternationalReserve.ID)
            {
                return BadRequest();
            }

            _context.Entry(grossInternationalReserve).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrossInternationalReserveExists(id))
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

        // POST: api/GrossInternationalReserves
        [HttpPost]
        public async Task<IActionResult> PostGrossInternationalReserve([FromBody] GrossInternationalReserve grossInternationalReserve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GrossInternationalReserves.Add(grossInternationalReserve);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrossInternationalReserve", new { id = grossInternationalReserve.ID }, grossInternationalReserve);
        }

        // DELETE: api/GrossInternationalReserves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrossInternationalReserve([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grossInternationalReserve = await _context.GrossInternationalReserves.SingleOrDefaultAsync(m => m.ID == id);
            if (grossInternationalReserve == null)
            {
                return NotFound();
            }

            _context.GrossInternationalReserves.Remove(grossInternationalReserve);
            await _context.SaveChangesAsync();

            return Ok(grossInternationalReserve);
        }

        private bool GrossInternationalReserveExists(int id)
        {
            return _context.GrossInternationalReserves.Any(e => e.ID == id);
        }
    }
}