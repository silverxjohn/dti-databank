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

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Roles")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LogHelper _logger;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
            _logger = new LogHelper(context);
        }

        // GET: api/Roles
        [HttpGet]
        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.SingleOrDefaultAsync(m => m.ID == id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole([FromRoute] int id, [FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.ID)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var roles = new Role();

            roles.Name = role.Name;
            
            var updatedRole = _context.Roles.FirstOrDefault(r => r.ID == id);

            _logger.Log(Logger.Action.Update, roles.Name);

            return Ok(updatedRole);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var roleToCreate = new Role
            {
                Name = role.Name,
                Description = role.Description
            };

            _context.Roles.Add(roleToCreate);

            var permissions = _context.Permission;

            foreach (var permission in permissions)
            {
                var permissionRoleToCreate = new PermissionRole
                {
                    PermissionID = permission.ID,
                    RoleID = roleToCreate.ID,
                    IsEnabled = false
                };
                _context.PermissionRole.Add(permissionRoleToCreate);
            }
            
            await _context.SaveChangesAsync();

            _logger.Log(Logger.Action.Create, role.Name);

            return CreatedAtAction("GetRole", new { id = roleToCreate.ID }, roleToCreate);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.SingleOrDefaultAsync(m => m.ID == id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            _logger.Log(Logger.Action.Delete, role.Name);

            return Ok(role);
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.ID == id);
        }
    }
}