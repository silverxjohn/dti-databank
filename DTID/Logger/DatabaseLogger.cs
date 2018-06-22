using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.Logger
{
    class DatabaseLogger : ILogger
    {
        private readonly ApplicationDbContext _context;

        public DatabaseLogger(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void Log(string message)
        {
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
