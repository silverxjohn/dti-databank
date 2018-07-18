using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using DTID.BusinessLogic.ViewModels.RoleViewModel;

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
                .Where(role => role.RoleID == id).Where(pr => pr.IsEnabled).ToList(); //change me
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
        public async Task<IActionResult> PutPermissionRole([FromRoute] int id, [FromBody] PermissionRoleDataViewModel permissions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permissions.ID)
            {
                return BadRequest();
            }

            foreach (var permission in permissions.Permissions)
            {
                var a = _context.PermissionRole.Where(pr => pr.PermissionID == permission.PermissionID).Where(pr => pr.RoleID == permission.RoleID).First();
                a.IsEnabled = permission.IsEnabled;
            }

            await _context.SaveChangesAsync();

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

            var permissionRoleToUpdate = _context.PermissionRole.Where(pR => pR.RoleID == permissionRole.RoleID && pR.PermissionID == permissionRole.PermissionID).First();

            permissionRoleToUpdate.IsEnabled = permissionRole.IsEnabled;

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