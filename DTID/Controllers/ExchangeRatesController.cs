using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.ExchangeRateViewModels;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/ExchangeRates")]
    public class ExchangeRatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExchangeRatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ExchangeRates
        [HttpGet]
        public IEnumerable<YearViewModel> GetExchangeRates()
        {
            var exchangeRates = _context.ExchangeRates;

            var rates = exchangeRates.Where(eRates => eRates.MonthID == null).Select(eRates => new YearViewModel
            {
                ID = eRates.ID,
                YearId = eRates.Year.ID,
                Name = eRates.Year.Name,
                Rate = eRates.Rate,
                Months = exchangeRates.Where(monthRate => monthRate.MonthID != null).Where(monthRate => monthRate.Year.ID == eRates.Year.ID).Select(monthRate => new MonthViewModel {
                    ID = monthRate.ID,
                    MonthId = monthRate.Month.ID,
                    Name = monthRate.Month.Name,
                    Rate = monthRate.Rate
                }).ToList()
            }).ToList();

            return rates;
        }

        // GET: api/ExchangeRates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exchangeRate = await _context.ExchangeRates.SingleOrDefaultAsync(m => m.ID == id);

            if (exchangeRate == null)
            {
                return NotFound();
            }

            return Ok(exchangeRate);
        }

        // PUT: api/ExchangeRates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExchangeRate([FromRoute] int id, [FromBody] ExchangeRate exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exchangeRate.ID)
            {
                return BadRequest();
            }

            _context.Entry(exchangeRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExchangeRateExists(id))
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

        // POST: api/ExchangeRates
        [HttpPost]
        public async Task<IActionResult> PostExchangeRate([FromBody] ExchangeRate exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExchangeRate", new { id = exchangeRate.ID }, exchangeRate);
        }

        // DELETE: api/ExchangeRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchangeRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exchangeRate = await _context.ExchangeRates.SingleOrDefaultAsync(m => m.ID == id);
            if (exchangeRate == null)
            {
                return NotFound();
            }

            _context.ExchangeRates.Remove(exchangeRate);
            await _context.SaveChangesAsync();

            return Ok(exchangeRate);
        }

        private bool ExchangeRateExists(int id)
        {
            return _context.ExchangeRates.Any(e => e.ID == id);
        }
    }
}