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
    [Route("api/ForApprovals")]
    public class ForApprovalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForApprovalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ForApprovals
        [HttpGet]
        public IEnumerable<ForApproval> GetForApproval()
        {
            return _context.ForApproval.Include(app => app.User).Include(app => app.CannedIndicator).ToList();
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

            if (id != forApproval.CannedIndicatorID)
            {
                return BadRequest();
            }

            var forApprovals = _context.ForApproval.Where(indicator => indicator.CannedIndicatorID == id);

            foreach (var forApp in forApprovals) {
                forApp.Comment = forApproval.Comment;
                forApp.isApproved = forApproval.isApproved;

                _context.Entry(forApp).State = EntityState.Modified;
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

            _context.ForApproval.Add(forApproval);
            await _context.SaveChangesAsync();

            int indicatorID = forApproval.CannedIndicatorID;

            var CannedIndicator = new CannedIndicator() {
                    ID = indicatorID,
                    Status = false
            };

            _context.Entry(CannedIndicator).Property(status => status.Status).IsModified = true;
            _context.SaveChanges();

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
    }
}