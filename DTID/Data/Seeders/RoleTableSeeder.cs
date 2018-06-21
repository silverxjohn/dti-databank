using DTID.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTID.Data.Seeders
{
    public class RoleTableSeeder : ISeeder
    {
        public readonly ApplicationDbContext _context;

        public RoleTableSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            var roles = new List<Role>();

            roles.Add(new Role
            {
                Name = "Admin"
            });

            roles.Add(new Role
            {
                Name = "Member"
            });


            _context.AddRange(roles);
            _context.SaveChanges();
        }
    }
}
