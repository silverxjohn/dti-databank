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
    [Route("api/IndicatorDatas")]
    public class IndicatorDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IndicatorDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IndicatorDatas
        [HttpGet]
        public IEnumerable<IndicatorData> GetIndicatorDatas()
        {
            return _context.IndicatorDatas;
        }

        // GET: api/IndicatorDatas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIndicatorData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var indicatorData = await _context.IndicatorDatas.Where(data => data.Indicator.ID == id).FirstAsync();

            if (indicatorData == null)
            {
                return NotFound();
            }

            return Ok(indicatorData);
        }

        // PUT: api/IndicatorDatas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndicatorData([FromRoute] int id, [FromBody] IndicatorData indicatorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != indicatorData.ID)
            {
                return BadRequest();
            }

            _context.Entry(indicatorData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicatorDataExists(id))
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

        // POST: api/IndicatorDatas
        [HttpPost]
        public async Task<IActionResult> PostIndicatorData([FromBody] IndicatorData indicatorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.IndicatorDatas.Add(indicatorData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndicatorData", new { id = indicatorData.ID }, indicatorData);
        }

        // DELETE: api/IndicatorDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndicatorData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var indicatorData = await _context.IndicatorDatas.SingleOrDefaultAsync(m => m.ID == id);
            if (indicatorData == null)
            {
                return NotFound();
            }

            _context.IndicatorDatas.Remove(indicatorData);
            await _context.SaveChangesAsync();

            return Ok(indicatorData);
        }

        private bool IndicatorDataExists(int id)
        {
            return _context.IndicatorDatas.Any(e => e.ID == id);
        }
    }
}