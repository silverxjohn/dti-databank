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
    [Route("api/CannedIndicators")]
    public class CannedIndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CannedIndicatorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CannedIndicators
        [HttpGet]
        public IEnumerable<CannedIndicator> GetCannedIndicator()
        {
            return _context.CannedIndicator;
        }

        // GET: api/CannedIndicators/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCannedIndicator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cannedIndicator = await _context.CannedIndicator.SingleOrDefaultAsync(m => m.ID == id);

            if (cannedIndicator == null)
            {
                return NotFound();
            }

            return Ok(cannedIndicator);
        }

        // PUT: api/CannedIndicators/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCannedIndicator([FromRoute] int id, [FromBody] CannedIndicator cannedIndicator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cannedIndicator.ID)
            {
                return BadRequest();
            }

            _context.Entry(cannedIndicator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CannedIndicatorExists(id))
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

        // POST: api/CannedIndicators
        [HttpPost]
        public async Task<IActionResult> PostCannedIndicator([FromBody] CannedIndicator cannedIndicator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CannedIndicator.Add(cannedIndicator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCannedIndicator", new { id = cannedIndicator.ID }, cannedIndicator);
        }

        // DELETE: api/CannedIndicators/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCannedIndicator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cannedIndicator = await _context.CannedIndicator.SingleOrDefaultAsync(m => m.ID == id);
            if (cannedIndicator == null)
            {
                return NotFound();
            }

            _context.CannedIndicator.Remove(cannedIndicator);
            await _context.SaveChangesAsync();

            return Ok(cannedIndicator);
        }

        private bool CannedIndicatorExists(int id)
        {
            return _context.CannedIndicator.Any(e => e.ID == id);
        }
    }
}