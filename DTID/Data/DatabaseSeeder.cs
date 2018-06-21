using DTID.Data.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTID.Data
{
    public static class DatabaseSeeder
    {
        private static List<ISeeder> Seeders { get; set; }

        private static void RegisterSeeders(ApplicationDbContext context)
        {
            Seeders = new List<ISeeder>
            {
                new RoleTableSeeder(context),
                new YearTableSeeder(context)
            };
        }

        public static void Populate(ApplicationDbContext context)
        {
            RegisterSeeders(context);

            foreach (ISeeder seeder in Seeders)
            {
                seeder.Seed();
            }
        }
    }
}
