using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DTID.Logger
{
    class DatabaseLogger : ILogger
    {
        private readonly ApplicationDbContext _context;
        private readonly ClaimsPrincipal _user;

        public DatabaseLogger(ApplicationDbContext context, ClaimsPrincipal user)
        {
            _context = context;
            _user = user;
        }
        
        public void Log(string message)
        {
            //var id = Int32.Parse(_user.Claims.FirstOrDefault(c => c.Type == "id").Value);
            var log = new Log
            {
                UserID = 1, //change me
                Action = message
            };

            _context.Logs.Add(log);

            _context.SaveChanges();
        }
    }
}
