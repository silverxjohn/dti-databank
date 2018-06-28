using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DTID.Services;

namespace DTID.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //var salt = Hash.CreateSalt();
            //var user = new User
            //{
            //    FirstName = "John",
            //    LastName = "Doe",
            //    Email = "jdoe@email.com",
            //    Password = Hash.Create("password", salt),
            //    Salt = salt,
            //    Contact = "afaw",
            //    Role = new Role
            //    {
            //        Name = "Admin",
            //        Description = "Admin"
            //    }
            //};

            //_context.Users.Add(user);

            //_context.SaveChanges();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
