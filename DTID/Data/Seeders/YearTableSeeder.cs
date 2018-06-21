using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTID.Data.Seeders
{
    public class YearTableSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public YearTableSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void Seed()
        {
            for (var i = 1990; i < 2019; i++)
            {
                _context.Years.Add(new BusinessLogic.Models.Year
                {
                    Name = i.ToString()
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
