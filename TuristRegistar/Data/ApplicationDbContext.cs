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
        public DbSet<SpecialOffersPrices> SpecialOffersPrices { get; set; }
        public DbSet<ObjectOffers> ObjectOffers { get; set; }
        public DbSet<ObjectTypes> ObjectTypes { get; set; }
        public DbSet<OccupancyBasedPrices> OccupancyBasedPrices { get; set; }
        public DbSet<OccupancyBasedPricing> OccupancyBasedPricings { get; set; }
        public DbSet<StandardPricingModel> StandardPricingModels { get; set; }
        public DbSet<RatingsAndReviews> RatingsAndReviews { get; set; }
        public DbSet<Objects> Objects { get; set; }
        public DbSet<Users> Userss { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Countries> Counries { get; set; }
        public DbSet<CntObjAttributesCount> CntObjAttributesCount { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<ObjectHasAttributes> ObjectHasAttributes { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //For composite PK
            modelBuilder.Entity<CntObjAttributesCount>()
                .HasKey(c => new { c.CountableObjAttrId, c.ObjectId });
            modelBuilder.Entity<SpecialOffersPrices>()
                .HasKey(c => new { c.SpecialOfferId, c.ObjectId });
            modelBuilder.Entity<ObjectHasAttributes>()
                .HasKey(o => new { o.ObjectId, o.AttributeId });

            #region "Seed data"

            //modelBuilder.Entity<IdentityRole>().HasData(
            //    new { Id = "1", Name="User", NormalizedName="USER"},
            //    new { Id = "2", Name = "Admin", NormalizedName ="ADMIN"}
            //    );
            //modelBuilder.Entity<Countries>().HasData(
            //    new { Id = 1, Name = "Bosna i Hercegovina" },
            //    new { Id = 2, Name = "Srbija" },
            //    new { Id = 3, Name = "Crna Gora" },
            //    new { Id = 4, Name = "Hrvatska" },
            //    new { Id = 5, Name = "Makedonija" },
            //    new { Id = 6, Name = "Slovenija" }
            //    );
            //modelBuilder.Entity<Cities>().HasData(
            //    new { Id = 13, Name = "Skoplje", CountriesId = 5 },
            //    new { Id = 14, Name = "Ohrid", CountriesId = 5 }
            //    );
            //modelBuilder.Entity<ObjectAttributes>().HasData(
            //   new { Id = 1, Name = "Wi-Fi" },
            //   new { Id = 2, Name = "Grijanje" },
            //   new { Id = 3, Name = "Doručak" },
            //   new { Id = 4, Name = "Mini-bar" }
            //   );


            #endregion
        }
    }
}
