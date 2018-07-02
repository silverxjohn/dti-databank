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
    [Route("api/BoiPezas")]
    public class BoiPezasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoiPezasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BoiPezas
        [HttpGet]
        public IEnumerable<BoiPeza> GetBoiPezas()
        {
            return _context.BoiPezas.Include(boipeza => boipeza.Year).GroupBy(boipeza => boipeza.YearId).Select(boipeza => boipeza.First());
        }

        // GET: api/BoiPezas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoiPeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boiPeza = await _context.BoiPezas.SingleOrDefaultAsync(m => m.ID == id);

            if (boiPeza == null)
            {
                return NotFound();
            }

            return Ok(boiPeza);
        }

        // PUT: api/BoiPezas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoiPeza([FromRoute] int id, [FromBody] BoiPeza boiPeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != boiPeza.ID)
            {
                return BadRequest();
            }

            _context.Entry(boiPeza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoiPezaExists(id))
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

        // POST: api/BoiPezas
        [HttpPost]
        public async Task<IActionResult> PostBoiPeza([FromBody] BoiPeza boiPeza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BoiPezas.Add(boiPeza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoiPeza", new { id = boiPeza.ID }, boiPeza);
        }

        // DELETE: api/BoiPezas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoiPeza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boiPeza = await _context.BoiPezas.SingleOrDefaultAsync(m => m.ID == id);
            if (boiPeza == null)
            {
                return NotFound();
            }

            _context.BoiPezas.Remove(boiPeza);
            await _context.SaveChangesAsync();

            return Ok(boiPeza);
        }

        private bool BoiPezaExists(int id)
        {
            return _context.BoiPezas.Any(e => e.ID == id);
        }
    }
}