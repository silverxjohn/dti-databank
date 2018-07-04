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
    [Route("api/PermissionRoles")]
    public class PermissionRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissionRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PermissionRoles
        [HttpGet]
        public IEnumerable<PermissionRole> GetPermissionRole()
        {
            return _context.PermissionRole;
        }

        [HttpGet("getPermission/{id}")]
        public IEnumerable<PermissionRole> GetPermissions([FromRoute] int id)
        {
            return _context.PermissionRole.Include(permission => permission.Permission)
                .Where(role => role.RoleID == id).ToList(); //change me
        }

        // GET: api/PermissionRoles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissionRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionRole = await _context.PermissionRole.SingleOrDefaultAsync(m => m.ID == id);

            if (permissionRole == null)
            {
                return NotFound();
            }

            return Ok(permissionRole);
        }

        // PUT: api/PermissionRoles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissionRole([FromRoute] int id, [FromBody] PermissionRole permissionRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permissionRole.ID)
            {
                return BadRequest();
            }

            _context.Entry(permissionRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionRoleExists(id))
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

        // POST: api/PermissionRoles
        [HttpPost]
        public async Task<IActionResult> PostPermissionRole([FromBody] PermissionRole permissionRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PermissionRole.Add(permissionRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermissionRole", new { id = permissionRole.ID }, permissionRole);
        }

        // DELETE: api/PermissionRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionRole = _context.PermissionRole.Where(pr => pr.RoleID == id);

            if (permissionRole == null)
            {
                return NotFound();
            }

            _context.PermissionRole.RemoveRange(permissionRole);

            await _context.SaveChangesAsync();

            return Ok(permissionRole);
        }

        private bool PermissionRoleExists(int id)
        {
            return _context.PermissionRole.Any(e => e.ID == id);
        }
    }
}