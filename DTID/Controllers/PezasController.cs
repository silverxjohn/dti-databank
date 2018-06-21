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
    [Route("api/Pezas")]
    public class PezasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PezasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pezas
        [HttpGet]
        public IEnumerable<Peza> GetPezas()
        {
            return _context.Pezas.Include(peza => peza.Year);
        }

        // GET: api/Pezas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var peza = await _context.Pezas.SingleOrDefaultAsync(m => m.ID == id);

            if (peza == null)
            {
                return NotFound();
            }

            return Ok(peza);
        }

        // PUT: api/Pezas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeza([FromRoute] int id, [FromBody] Peza peza)
        {
            var pezaToUpdate = _context.Pezas.FirstOrDefault(pezas => pezas.ID == id);

            if (pezaToUpdate == null)
            {
                return NotFound();
            }

            pezaToUpdate.YearId = peza.YearId;

            pezaToUpdate.Amount = peza.Amount;

            await _context.SaveChangesAsync();

            _context.Entry(peza).State = EntityState.Modified;

            return Ok(pezaToUpdate);
        }

        // POST: api/Pezas
        [HttpPost]
        public async Task<IActionResult> PostPeza([FromBody] Peza peza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pezas.Add(peza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPeza", new { id = peza.ID }, peza);
        }

        // DELETE: api/Pezas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var peza = await _context.Pezas.SingleOrDefaultAsync(m => m.ID == id);
            if (peza == null)
            {
                return NotFound();
            }

            _context.Pezas.Remove(peza);
            await _context.SaveChangesAsync();

            return Ok(peza);
        }

        private bool PezaExists(int id)
        {
            return _context.Pezas.Any(e => e.ID == id);
        }
    }
}