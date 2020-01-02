using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AvailablePeriods> AvailablePeriods { get; set; }
        public DbSet<CountableObjectAttributes> CountableObjectAttributes { get; set; }
        public DbSet<ObjectAttributes> ObjectAttributes { get; set; }
        public DbSet<ObjectImages> ObjectImages { get; set; }
        public DbSet<SpecialOffers> SpecialOffers { get; set; }
        public DbSet<ObjectOffers> ObjectOffers { get; set; }
        public DbSet<ObjectTypes> ObjectTypes { get; set; }
        public DbSet<OccupancyBasedPrices> OccupancyBasedPrices { get; set; }
        public DbSet<OccupancyBasedPricing> OccupancyBasedPricings { get; set; }
        public DbSet<StandardPricingModel> StandardPricingModels { get; set; }
        public DbSet<RatingsAndReviews> RatingsAndReviews { get; set; }
        public DbSet<Objects> Objects { get; set; }
        public DbSet<Users> Userss { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //For composite PK
            //modelBuilder.Entity<UserRoles>()
            //    .HasKey(c => new { c.RoleId, c.UserId });

            #region "Seed data"

            modelBuilder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name="User", NormalizedName="USER"},
                new { Id = "2", Name = "Admin", NormalizedName ="ADMIN"}
                );

            #endregion
        }
    }
}
