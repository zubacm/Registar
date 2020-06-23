﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TuristRegistar.Data;

namespace TuristRegistar.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200608212224_someseeding")]
    partial class someseeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new { Id = "3", Name = "BANNED", NormalizedName = "BANNED" }
                    );
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Bookmark", b =>
                {
                    b.Property<int>("ObjectId");

                    b.Property<string>("UserId");

                    b.Property<string>("UsersId");

                    b.HasKey("ObjectId", "UserId");

                    b.HasIndex("UserId");

                    b.HasIndex("UsersId");

                    b.ToTable("Bookmark");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountriesId");

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountriesId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.CntObjAttributesCount", b =>
                {
                    b.Property<int>("CountableObjAttrId");

                    b.Property<int>("ObjectId");

                    b.Property<int>("Count");

                    b.HasKey("CountableObjAttrId", "ObjectId");

                    b.HasIndex("ObjectId");

                    b.ToTable("CntObjAttributesCount");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.CountableObjectAttributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CountableObjectAttributes");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Countries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Counries");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Currencies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Key");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectAttributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ObjectAttributes");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectHasAttributes", b =>
                {
                    b.Property<int>("ObjectId");

                    b.Property<int>("AttributeId");

                    b.HasKey("ObjectId", "AttributeId");

                    b.HasAlternateKey("AttributeId", "ObjectId");

                    b.ToTable("ObjectHasAttributes");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCover");

                    b.Property<int?>("ObjectsId");

                    b.Property<string>("Path");

                    b.HasKey("Id");

                    b.HasIndex("ObjectsId");

                    b.ToTable("ObjectImages");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectOffers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IncludedInOriginalPrice");

                    b.Property<float>("Price");

                    b.Property<int>("SpecialOfferId");

                    b.Property<int?>("SpecialOffersPricesObjectId");

                    b.Property<int?>("SpecialOffersPricesSpecialOfferId");

                    b.HasKey("Id");

                    b.HasIndex("SpecialOffersPricesSpecialOfferId", "SpecialOffersPricesObjectId");

                    b.ToTable("ObjectOffers");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Objects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int?>("CityId");

                    b.Property<int?>("CountryId");

                    b.Property<int?>("CreatorId");

                    b.Property<string>("CreatorId1");

                    b.Property<string>("Description");

                    b.Property<string>("EmailContact")
                        .HasMaxLength(256);

                    b.Property<string>("FullAddress");

                    b.Property<string>("IdentUserId");

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<string>("Name");

                    b.Property<int?>("ObjectTypeId");

                    b.Property<int?>("OccupancyBasedPricingId");

                    b.Property<bool>("OccupancyPricing");

                    b.Property<string>("PhoneNumberContact");

                    b.Property<int?>("StandardPricingModelId");

                    b.Property<float?>("Surface");

                    b.Property<string>("WebContact");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("CreatorId1");

                    b.HasIndex("IdentUserId");

                    b.HasIndex("ObjectTypeId");

                    b.HasIndex("OccupancyBasedPricingId")
                        .IsUnique()
                        .HasFilter("[OccupancyBasedPricingId] IS NOT NULL");

                    b.HasIndex("StandardPricingModelId")
                        .IsUnique()
                        .HasFilter("[StandardPricingModelId] IS NOT NULL");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ObjectTypes");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.OccupancyBasedPrices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OccunapncyBasedPricingId");

                    b.Property<int>("Occupancy");

                    b.Property<decimal>("PricePerNight");

                    b.HasKey("Id");

                    b.HasIndex("OccunapncyBasedPricingId");

                    b.ToTable("OccupancyBasedPrices");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.OccupancyBasedPricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MaxDaysOffer");

                    b.Property<int?>("MaxOccupancy");

                    b.Property<int?>("MinDaysOffer");

                    b.Property<int?>("MinOccupancy");

                    b.HasKey("Id");

                    b.ToTable("OccupancyBasedPricings");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.RatingsAndReviews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ObjectId");

                    b.Property<int>("Rating");

                    b.Property<string>("Review");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ObjectId");

                    b.HasIndex("UserId");

                    b.ToTable("RatingsAndReviews");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.SpecialOffersPrices", b =>
                {
                    b.Property<int>("SpecialOfferId");

                    b.Property<int>("ObjectId");

                    b.Property<decimal>("Price");

                    b.HasKey("SpecialOfferId", "ObjectId");

                    b.HasAlternateKey("ObjectId", "SpecialOfferId");

                    b.ToTable("SpecialOffersPrices");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.StandardPricingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MaxDaysOffer");

                    b.Property<int?>("MaxOccupancy");

                    b.Property<int?>("MinDaysOffer");

                    b.Property<int?>("MinOccupancy");

                    b.Property<decimal?>("OffsetPercentage");

                    b.Property<int?>("StandardOccupancy");

                    b.Property<decimal?>("StandardPricePerNight");

                    b.HasKey("Id");

                    b.ToTable("StandardPricingModels");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.UnavailablePeriods", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("From");

                    b.Property<int?>("ObjectsId");

                    b.Property<DateTime>("To");

                    b.HasKey("Id");

                    b.HasIndex("ObjectsId");

                    b.ToTable("AvailablePeriods");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Users", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ContactAddress");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<string>("IdentUserId");

                    b.Property<string>("LastName")
                        .HasMaxLength(256);

                    b.Property<bool>("LegalPerson");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IdentUserId");

                    b.ToTable("Userss");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Bookmark", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Objects", "Object")
                        .WithMany()
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TuristRegistar.Data.Models.Users")
                        .WithMany("ObjectHasAttributes")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Cities", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Countries")
                        .WithMany("Cities")
                        .HasForeignKey("CountriesId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.CntObjAttributesCount", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.CountableObjectAttributes", "CountableObjAttr")
                        .WithMany()
                        .HasForeignKey("CountableObjAttrId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TuristRegistar.Data.Models.Objects", "Object")
                        .WithMany("CntObjAttributesCount")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectHasAttributes", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.ObjectAttributes", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TuristRegistar.Data.Models.Objects", "Object")
                        .WithMany("ObjectHasAttributes")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectImages", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Objects", "Objects")
                        .WithMany("ObjectImages")
                        .HasForeignKey("ObjectsId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.ObjectOffers", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.SpecialOffersPrices", "SpecialOffersPrices")
                        .WithMany()
                        .HasForeignKey("SpecialOffersPricesSpecialOfferId", "SpecialOffersPricesObjectId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Objects", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Cities", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("TuristRegistar.Data.Models.Countries", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("TuristRegistar.Data.Models.Users", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId1");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentUser")
                        .WithMany()
                        .HasForeignKey("IdentUserId");

                    b.HasOne("TuristRegistar.Data.Models.ObjectTypes", "ObjectType")
                        .WithMany()
                        .HasForeignKey("ObjectTypeId");

                    b.HasOne("TuristRegistar.Data.Models.OccupancyBasedPricing", "OccupancyBasedPricing")
                        .WithOne("Objects")
                        .HasForeignKey("TuristRegistar.Data.Models.Objects", "OccupancyBasedPricingId");

                    b.HasOne("TuristRegistar.Data.Models.StandardPricingModel", "StandardPricingModel")
                        .WithOne("Objects")
                        .HasForeignKey("TuristRegistar.Data.Models.Objects", "StandardPricingModelId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.OccupancyBasedPrices", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.OccupancyBasedPricing", "OccunapncyBasedPricing")
                        .WithMany("Prices")
                        .HasForeignKey("OccunapncyBasedPricingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.RatingsAndReviews", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Objects", "Object")
                        .WithMany("RatingsAndReviews")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TuristRegistar.Data.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.SpecialOffersPrices", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Objects", "Object")
                        .WithMany("SpecialOffers")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TuristRegistar.Data.Models.ObjectAttributes", "SpecialOffer")
                        .WithMany()
                        .HasForeignKey("SpecialOfferId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.UnavailablePeriods", b =>
                {
                    b.HasOne("TuristRegistar.Data.Models.Objects", "Objects")
                        .WithMany("UnavailablePeriods")
                        .HasForeignKey("ObjectsId");
                });

            modelBuilder.Entity("TuristRegistar.Data.Models.Users", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentUser")
                        .WithMany()
                        .HasForeignKey("IdentUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
