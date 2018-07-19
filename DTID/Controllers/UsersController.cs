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
using DTID.Services;
using DTID.BusinessLogic.ViewModels.UserViewModels;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private LogHelper _logger;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.Users.Include(user => user.Role).ToList();
        }

        [HttpGet("getPermission")]
        public IEnumerable<PermissionRole> GetPermissions()
        {
            return _context.PermissionRole.Include(permission => permission.Permission)
                .Where(role => role.RoleID == 1).Where(permission => permission.Permission.Name
                .ToString().Contains("accounts")).ToList(); //change me
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody] ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.Find(id);
            user.Password = Hash.Create(vm.Password, user.Salt);

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger = new LogHelper(_context, vm.UserID);

            _logger.Log(Logger.Action.Update, user);

            return Ok();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vm.ID)
            {
                return BadRequest();
            }

            var user = new User
            {
                ID = vm.ID,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                Email = vm.Email,
                Password = vm.Password,
                Salt = vm.Salt,
                Contact = vm.Contact,
                RoleID = vm.RoleID,
                Role = vm.Role,
                DateCreated = vm.DateCreated,
                DateUpdated = vm.DateUpdated
            };

            var account = _context.Users.Find(id);
            account.Contact = user.Contact;
            account.Email = user.Email;
            account.FirstName = user.FirstName;
            account.LastName = user.LastName;
            account.RoleID = user.RoleID;

            _context.Entry(account).State = EntityState.Modified;

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

            var updatedUser = _context.Users.Include(u => u.Role).FirstOrDefault(r => r.ID == id);

            _logger = new LogHelper(_context, vm.UserID);

            _logger.Log(Logger.Action.Update, updatedUser);

            return Ok(updatedUser);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                ID = vm.ID,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                Email = vm.Email,
                Password = vm.Password,
                Salt = vm.Salt,
                Contact = vm.Contact,
                RoleID = vm.RoleID,
                Role = vm.Role,
                DateCreated = vm.DateCreated,
                DateUpdated = vm.DateUpdated
            };

            user.Salt = Hash.CreateSalt();
            user.Password = Hash.Create(user.Password, user.Salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var user1 = _context.Users.Include(u => u.Role).SingleOrDefault(u => u.ID == user.ID);

            _logger = new LogHelper(_context, vm.UserID);

            _logger.Log(Logger.Action.Create, user1);

            return CreatedAtAction("GetUser", new { id = user.ID }, user1);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id, [FromBody] DeleteUserViewModel vm)
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

            _logger = new LogHelper(_context, vm.UserID);

            _logger.Log(Logger.Action.Delete, user);

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}