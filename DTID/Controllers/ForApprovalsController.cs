using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using System.Security.Claims;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/ForApprovals")]
    public class ForApprovalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForApprovalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ForApprovals
        [HttpGet("{id}/User/{roleId}")]
        public IEnumerable<ForApproval> GetForApproval([FromRoute] int id, [FromRoute] int roleId)
        {
            var canApprove = _context.PermissionRole.Where(pr => pr.RoleID == roleId).Select(pr => pr.Permission).Any(pr => pr.ID == 25);

            var canModify = _context.PermissionRole.Where(pr => pr.RoleID == roleId).Select(pr => pr.Permission).Any(pr => pr.ID == 36);

            var requests = new List<ForApproval>();

            if (canApprove)
            {
                requests = _context.ForApproval
                    .Include(app => app.User)
                    .Include(app => app.CannedIndicator)
                    .Include(app => app.Indicator)
                    .OrderByDescending(i => i.DateCreated)
                    .GroupBy(i => i.ID)
                    .Select(g => g.First())
                    .ToList();
            } else
            {
                requests = _context.ForApproval
                    .Where(app => app.UserID == id)
                    .Include(app => app.User)
                    .Include(app => app.CannedIndicator)
                    .Include(app => app.Indicator)
                    .OrderByDescending(i => i.ID)
                    .GroupBy(i => i.IndicatorID)
                    .Select(g => g.First())
                    .ToList();
            }


            return requests;
        }

        // GET: api/ForApprovals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForApproval([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forApproval = await _context.ForApproval.SingleOrDefaultAsync(m => m.ID == id);

            if (forApproval == null)
            {
                return NotFound();
            }

            return Ok(forApproval);
        }

        // PUT: api/ForApprovals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForApproval([FromRoute] int id, [FromBody] ForApproval forApproval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forApproval.ID)
            {
                return BadRequest();
            }

            var forApprovals = _context.ForApproval.Where(indicator => indicator.ID == id);
            foreach (var forApp in forApprovals) {
                forApp.UserID = forApproval.UserID;
                forApp.isApproved = forApproval.isApproved;
                forApp.Comment = forApproval.Comment;
                forApp.DateCreated = DateTime.Now;

                _context.Entry(forApp).State = EntityState.Modified;
            }
            
            if (forApproval.isApproved == ApprovalStatus.Approved)
            {
                if (forApproval.CannedIndicatorID != null)
                {
                    UpdateIndicators((int)forApproval.CannedIndicatorID);
                } else if (forApproval.IndicatorID != null)
                {
                    var indicator = _context.Indicators.Find(forApproval.IndicatorID);
                    indicator.IsApproved = forApproval.isApproved == ApprovalStatus.Approved;

                    _context.Entry(indicator).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForApprovalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updateApproval = _context.ForApproval.Include(app => app.User).Include(app => app.CannedIndicator).ToList();

            return Ok(updateApproval);
        }

        // POST: api/ForApprovals
        [HttpPost]
        public async Task<IActionResult> PostForApproval([FromBody] ForApproval forApproval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //forApproval.UserID = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value);

            if (forApproval.CannedIndicatorID > 0)
            {
                int indicatorID = (int)forApproval.CannedIndicatorID;

                var CannedIndicator = new CannedIndicator()
                {
                    ID = indicatorID,
                    Status = false
                };

                _context.Entry(CannedIndicator).Property(status => status.Status).IsModified = true;
            } else if (forApproval.IndicatorID != null)
            {
                forApproval.CannedIndicatorID = null;

                var indicator = _context.Indicators.Find(forApproval.IndicatorID);
                indicator.IsApproved = false;

                _context.Entry(indicator).State = EntityState.Modified;
            }

            _context.ForApproval.Add(forApproval);

            try
            {
                await _context.SaveChangesAsync();
            } catch (Exception e)
            {

            }

            return CreatedAtAction("GetForApproval", new { id = forApproval.ID }, forApproval);
        }

        // DELETE: api/ForApprovals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForApproval([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forApproval = await _context.ForApproval.SingleOrDefaultAsync(m => m.ID == id);
            if (forApproval == null)
            {
                return NotFound();
            }

            _context.ForApproval.Remove(forApproval);
            await _context.SaveChangesAsync();

            return Ok(forApproval);
        }

        private bool ForApprovalExists(int id)
        {
            return _context.ForApproval.Any(e => e.ID == id);
        }

        private void UpdateIndicators(int id)
        {
            switch (id)
            {
                case (int)CannedIndicatorID.Wage:
                    var wages = _context.Wages.Where(wage => !wage.IsApproved);

                    foreach (var wage in wages)
                    {
                        wage.IsApproved = true;
                        _context.Entry(wage).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.Populations:
                    var populations = _context.Populations.Where(population => !population.IsApproved);

                    foreach (var population in populations)
                    {
                        population.IsApproved = true;
                        _context.Entry(population).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.InflationRate:
                    var inflationRates = _context.InflationRates.Where(iRate => !iRate.IsApproved);

                    foreach (var inflationRate in inflationRates)
                    {
                        inflationRate.IsApproved = true;
                        _context.Entry(inflationRate).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.GrossInternationalReserves:
                    var grossInternationalReserves = _context.GrossInternationalReserves.Where(GIR => !GIR.IsApproved);

                    foreach (var grossInternationalReserve in grossInternationalReserves)
                    {
                        grossInternationalReserve.IsApproved = true;
                        _context.Entry(grossInternationalReserve).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.ExchangeRate:
                    var exchangeRates = _context.ExchangeRates.Where(eRate => !eRate.IsApproved);

                    foreach (var exchangeRate in exchangeRates)
                    {
                        exchangeRate.IsApproved = true;
                        _context.Entry(exchangeRate).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.BalanceOfPayment:
                    var balanceOfPayments = _context.BalanceOfPayments.Where(BOP => !BOP.IsApproved);

                    foreach (var balanceOfPayment in balanceOfPayments)
                    {
                        balanceOfPayment.IsApproved = true;
                        _context.Entry(balanceOfPayment).State = EntityState.Modified;
                    }
                    break;

                case (int)CannedIndicatorID.BoiPeza:
                    var boiPezas = _context.BoiPezas.Where(boiPeza => !boiPeza.IsApproved);

                    foreach (var boiPeza in boiPezas)
                    {
                        boiPeza.IsApproved = true;
                        _context.Entry(boiPeza).State = EntityState.Modified;
                    }
                    break;

                default:
                    return;
            }
        }
    }
}