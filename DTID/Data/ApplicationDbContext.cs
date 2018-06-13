using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DTID.BusinessLogic.Models;
using System.Threading;

namespace DTID.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<ColumnValues> ColumnValues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<QuarterYear> QuarterYears { get; set; }
        public DbSet<BoardOfInvestment> BoardOfInvestments { get; set; }
        public DbSet<BalanceOfPayment> BalanceOfPayments { get; set; }
        public DbSet<Peza> Pezas { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<GrossInternationalReserve> GrossInternationalReserves { get; set; }
        public DbSet<InflationRate> InflationRates { get; set; }
        public DbSet<Population> Populations { get; set; }
        public DbSet<Wage> Wages { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public new DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TouchTimestamps();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            TouchTimestamps();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            TouchTimestamps();

            return base.SaveChanges();
        }

        private void TouchTimestamps()
        {
            DateTime currentTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DateCreated").CurrentValue = currentTime;

                entry.Property("DateUpdated").CurrentValue = currentTime;
            }
        }
    }
}
