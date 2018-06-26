using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Cors;

namespace DTID.Controllers
{
    [Produces("application/json")]
    [Route("api/Logs")]

    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Logs
        [HttpGet]
        public IEnumerable<Log> GetLogs()
        {
            return _context.Logs.Include(log => log.Users).ToList();
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogs([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logs = await _context.Logs.SingleOrDefaultAsync(m => m.ID == id);

            if (logs == null)
            {
                return NotFound();
            }

            return Ok(logs);
        }

        [HttpGet("getPermission")]
        public IEnumerable<PermissionRole> GetPermissions()
        {
            return _context.PermissionRole.Include(permission => permission.Permission)
                .Where(role => role.RoleID == 1).Where(permission => permission.Permission.Name
                .ToString().Contains("logs")).ToList(); //change me
        }
    }
}