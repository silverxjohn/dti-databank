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
    [Route("api/BoardOfInvestments")]
    public class BoardOfInvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardOfInvestmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BoardOfInvestments
        [HttpGet]
        public IEnumerable<BoardOfInvestment> GetBoardOfInvestments()
        {
            return _context.BoardOfInvestments.Include(boi => boi.Year);
        }

        // GET: api/BoardOfInvestments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardOfInvestment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boardOfInvestment = await _context.BoardOfInvestments.SingleOrDefaultAsync(m => m.ID == id);

            if (boardOfInvestment == null)
            {
                return NotFound();
            }

            return Ok(boardOfInvestment);
        }

        // PUT: api/BoardOfInvestments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoardOfInvestment([FromRoute] int id, [FromBody] BoardOfInvestment boardOfInvestment)
        {
            var boiToUpdate = _context.BoardOfInvestments.FirstOrDefault(bois => bois.ID == id);

            if (boiToUpdate == null)
            {
                return NotFound();
            }

            boiToUpdate.YearId = boardOfInvestment.YearId;

            boiToUpdate.Amount = boardOfInvestment.Amount;

            await _context.SaveChangesAsync();

            _context.Entry(boardOfInvestment).State = EntityState.Modified;

            return Ok(boiToUpdate);
        }

        // POST: api/BoardOfInvestments
        [HttpPost]
        public async Task<IActionResult> PostBoardOfInvestment([FromBody] BoardOfInvestment boardOfInvestment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BoardOfInvestments.Add(boardOfInvestment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoardOfInvestment", new { id = boardOfInvestment.ID }, boardOfInvestment);
        }

        // DELETE: api/BoardOfInvestments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoardOfInvestment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boardOfInvestment = await _context.BoardOfInvestments.SingleOrDefaultAsync(m => m.ID == id);
            if (boardOfInvestment == null)
            {
                return NotFound();
            }

            _context.BoardOfInvestments.Remove(boardOfInvestment);
            await _context.SaveChangesAsync();

            return Ok(boardOfInvestment);
        }

        private bool BoardOfInvestmentExists(int id)
        {
            return _context.BoardOfInvestments.Any(e => e.ID == id);
        }
    }
}