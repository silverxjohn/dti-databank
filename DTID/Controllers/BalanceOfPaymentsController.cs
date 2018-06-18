using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.BalanceOfPaymentViewModels;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/BalanceOfPayments")]
    public class BalanceOfPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BalanceOfPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BalanceOfPayments
        [HttpGet]
        public List<YearViewModel> GetBalanceOfPayments()
        {
            var data = _context.BalanceOfPayments;

            var balanceOfPayments = data.Where(yearBops => yearBops.MonthID == null).Where(yearBops => yearBops.QuarterID == null).Select(yearBops => new YearViewModel
            {
                ID = yearBops.ID,
                YearId = yearBops.Year.ID,
                Name = yearBops.Year.Name,
                BalanceOfPayments = yearBops.BalanceOfPayments,
                Months = data.Where(monthBops => monthBops.QuarterID == null).Where(monthBops => monthBops.MonthID != null).Where(monthBops => monthBops.Year.ID == yearBops.Year.ID).Select(monthBops => new MonthViewModel
                {
                    ID = monthBops.ID,
                    MonthId = monthBops.Month.ID,
                    Name = monthBops.Month.Name,
                    BalanceOfPayments = monthBops.BalanceOfPayments
                }).ToList(),
                Quarters = data.Where(quarterBops => quarterBops.MonthID == null).Where(quarterBops => quarterBops.QuarterID != null).Where(quarterBops => quarterBops.Year.ID == yearBops.Year.ID).Select(quarterBops => new QuarterViewModel
                {
                    ID = quarterBops.ID,
                    QuarterId = quarterBops.Quarter.ID,
                    Name = quarterBops.Quarter.Name,
                    BalanceOfPayments = quarterBops.BalanceOfPayments
                }).ToList()
            }).ToList();

            return balanceOfPayments;
        }

        // GET: api/BalanceOfPayments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBalanceOfPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanceOfPayment = await _context.BalanceOfPayments.SingleOrDefaultAsync(m => m.ID == id);

            if (balanceOfPayment == null)
            {
                return NotFound();
            }

            return Ok(balanceOfPayment);
        }

        // PUT: api/BalanceOfPayments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBalanceOfPayment([FromRoute] int id, [FromBody] BalanceOfPayment balanceOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != balanceOfPayment.ID)
            {
                return BadRequest();
            }

            _context.Entry(balanceOfPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BalanceOfPaymentExists(id))
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

        // POST: api/BalanceOfPayments
        [HttpPost]
        public async Task<IActionResult> PostBalanceOfPayment([FromBody] BalanceOfPayment balanceOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BalanceOfPayments.Add(balanceOfPayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBalanceOfPayment", new { id = balanceOfPayment.ID }, balanceOfPayment);
        }

        // DELETE: api/BalanceOfPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBalanceOfPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanceOfPayment = await _context.BalanceOfPayments.SingleOrDefaultAsync(m => m.ID == id);
            if (balanceOfPayment == null)
            {
                return NotFound();
            }

            _context.BalanceOfPayments.Remove(balanceOfPayment);
            await _context.SaveChangesAsync();

            return Ok(balanceOfPayment);
        }

        private bool BalanceOfPaymentExists(int id)
        {
            return _context.BalanceOfPayments.Any(e => e.ID == id);
        }
    }
}