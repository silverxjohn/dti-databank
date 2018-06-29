using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.GrossInternationalReserveViewModels;

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
        public IEnumerable<YearViewModel> GetGrossInternationalReserves()
        {
            var girs = _context.GrossInternationalReserves.Include(ex => ex.Month).Include(ez => ez.Year);

            var data = new Dictionary<string, object>();

            var ratess = girs.GroupBy(x => x.YearID).ToList();

            var rates = ratess.SelectMany(r =>
            {
                var vm = new List<YearViewModel>();
                foreach (var eRates in r)
                {
                    vm.Add(new YearViewModel
                    {
                        ID = eRates.ID,
                        YearId = eRates.Year.ID,
                        MonthId = eRates.Month.ID,
                        Name = eRates.Year.Name,
                        Rate = eRates.Rate,
                        Months = girs.Where(monthRate => monthRate.Year.ID == eRates.Year.ID).Select(monthRate => new MonthViewModel
                        {
                            ID = monthRate.ID,
                            MonthId = monthRate.Month.ID,
                            YearName = eRates.Year.Name,
                            YearId = eRates.Year.ID,
                            Name = monthRate.Month.Name,
                            Rate = monthRate.Rate
                        }).GroupBy(e => e.MonthId).Select(z => z.First()).ToList()
                    });
                }
                return vm;
            }).GroupBy(y => y.YearId).Select(g => g.First());

            return rates;
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