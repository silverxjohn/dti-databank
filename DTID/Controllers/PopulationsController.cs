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
    [Route("api/Populations")]
    public class PopulationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PopulationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Populations
        [HttpGet]
        public IEnumerable<Population> GetPopulations()
        {
            return _context.Populations.Include(population => population.Year).GroupBy(z => z.YearID).Select(r => r.First());
        }

        // GET: api/Populations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPopulation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var population = await _context.Populations.SingleOrDefaultAsync(m => m.ID == id);

            if (population == null)
            {
                return NotFound();
            }

            return Ok(population);
        }

        // PUT: api/Populations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPopulation([FromRoute] int id, [FromBody] Population population)
        {
            var populationToUpdate = _context.Populations.FirstOrDefault(populations => populations.ID == id);

            if (populationToUpdate == null)
            {
                return NotFound();
            }

            populationToUpdate.YearID = population.YearID;

            populationToUpdate.Populations = population.Populations;

            await _context.SaveChangesAsync();

            _context.Entry(population).State = EntityState.Modified;

            return Ok(populationToUpdate);
        }

        // POST: api/Populations
        [HttpPost]
        public async Task<IActionResult> PostPopulation([FromBody] Population population)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Populations.Add(population);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPopulation", new { id = population.ID }, population);
        }

        // DELETE: api/Populations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePopulation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var population = await _context.Populations.SingleOrDefaultAsync(m => m.ID == id);
            if (population == null)
            {
                return NotFound();
            }

            _context.Populations.Remove(population);
            await _context.SaveChangesAsync();

            return Ok(population);
        }

        private bool PopulationExists(int id)
        {
            return _context.Populations.Any(e => e.ID == id);
        }
    }
}