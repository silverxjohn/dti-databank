using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.Logger;
using Microsoft.AspNetCore.Cors;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LogHelper _logger;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
            _logger = new LogHelper(context);
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.Users.Include(user => user.Role).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedUser = _context.Users.FirstOrDefault(r => r.ID == id);

            _logger.Log(Logger.Action.Update, updatedUser);

            return Ok(updatedUser);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var user1 = _context.Users.Include(u => u.Role).SingleOrDefault(u => u.ID == user.ID);

            _logger.Log(Logger.Action.Create, user1);

            return CreatedAtAction("GetUser", new { id = user.ID }, user1);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.Log(Logger.Action.Delete, user);

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}