using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.InflationRateViewModels;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/InflationRates")]
    public class InflationRatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InflationRatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InflationRates
        [HttpGet]
        public List<YearViewModel> GetInflationRates()
        {
            var inflationRates = _context.InflationRates;

            var rates = inflationRates.Where(rate => rate.Month == null).Select(rate => new YearViewModel
            {
                ID = rate.ID,
                YearId = rate.Year.ID,
                Name = rate.Year.Name,
                Rate = rate.Rate,
                Months = inflationRates.Where(monthRate => monthRate.Month != null).Where(monthRate => monthRate.Year.ID == rate.Year.ID).Select(monthRate => new MonthViewModel
                {
                    ID = monthRate.ID,
                    MonthId = monthRate.Month.ID,
                    Name = monthRate.Month.Name,
                    Rate = monthRate.Rate
                }).ToList()
            }).ToList();

            return rates;
    
        }

        // GET: api/InflationRates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInflationRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inflationRate = await _context.InflationRates.SingleOrDefaultAsync(m => m.ID == id);

            if (inflationRate == null)
            {
                return NotFound();
            }

            return Ok(inflationRate);
        }

        // PUT: api/InflationRates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInflationRate([FromRoute] int id, [FromBody] InflationRate inflationRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inflationRate.ID)
            {
                return BadRequest();
            }

            _context.Entry(inflationRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InflationRateExists(id))
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

        // POST: api/InflationRates
        [HttpPost]
        public async Task<IActionResult> PostInflationRate([FromBody] InflationRate inflationRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.InflationRates.Add(inflationRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInflationRate", new { id = inflationRate.ID }, inflationRate);
        }

        // DELETE: api/InflationRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInflationRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inflationRate = await _context.InflationRates.SingleOrDefaultAsync(m => m.ID == id);
            if (inflationRate == null)
            {
                return NotFound();
            }

            _context.InflationRates.Remove(inflationRate);
            await _context.SaveChangesAsync();

            return Ok(inflationRate);
        }

        private bool InflationRateExists(int id)
        {
            return _context.InflationRates.Any(e => e.ID == id);
        }
    }
}