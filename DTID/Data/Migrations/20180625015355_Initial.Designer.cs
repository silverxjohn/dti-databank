﻿// <auto-generated />
using DTID.BusinessLogic.Models;
using DTID.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DTID.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180625015355_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DTID.BusinessLogic.Models.ApplicationUser", b =>
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

            modelBuilder.Entity("DTID.BusinessLogic.Models.BalanceOfPayment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BalanceOfPayments");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("MonthID");

                    b.Property<int?>("QuarterID");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("MonthID");

                    b.HasIndex("QuarterID");

                    b.HasIndex("YearID");

                    b.ToTable("BalanceOfPayments");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.BoardOfInvestment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("YearID");

                    b.ToTable("BoardOfInvestments");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<int?>("IndicatorID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("IndicatorID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Column", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryID");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Columns");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.ColumnValues", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ColumnID");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int>("RowId");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.HasIndex("ColumnID");

                    b.ToTable("ColumnValues");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Directory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Label");

                    b.Property<int?>("ParentID");

                    b.HasKey("ID");

                    b.HasIndex("ParentID");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.ExchangeRate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("MonthID");

                    b.Property<double>("Rate");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("MonthID");

                    b.HasIndex("YearID");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.GrossInternationalReserve", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("MonthID");

                    b.Property<double>("Rate");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("MonthID");

                    b.HasIndex("YearID");

                    b.ToTable("GrossInternationalReserves");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Indicator", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<int?>("FileID");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentID");

                    b.HasKey("ID");

                    b.HasIndex("FileID");

                    b.HasIndex("ParentID");

                    b.ToTable("Indicators");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Industry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("QuarterID");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("QuarterID");

                    b.HasIndex("YearID");

                    b.ToTable("Industrys");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.InflationRate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("MonthID");

                    b.Property<double>("Rate");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("MonthID");

                    b.HasIndex("YearID");

                    b.ToTable("InflationRates");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Log", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Month", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Months");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Peza", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("YearID");

                    b.ToTable("Pezas");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Population", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int>("Populations");

                    b.Property<int>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("YearID");

                    b.ToTable("Populations");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.PurchasingManagerIndex", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CompositeIndex");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int?>("IndustryID");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("MonthID");

                    b.Property<double>("PMI");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("IndustryID");

                    b.HasIndex("MonthID");

                    b.HasIndex("YearID");

                    b.ToTable("PurchasingManagerIndexs");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Quarter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Quarters");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.QuarterYear", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("QuarterID");

                    b.Property<int?>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("QuarterID");

                    b.HasIndex("YearID");

                    b.ToTable("QuarterYears");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.SourceFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUploaded");

                    b.Property<string>("Name");

                    b.Property<string>("OriginalName");

                    b.HasKey("ID");

                    b.ToTable("SourceFiles");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.SubIndustry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int?>("IndustryID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("IndustryID");

                    b.ToTable("SubIndustrys");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Contact");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<int>("RoleID");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Wage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<int>("Wages");

                    b.Property<int>("YearID");

                    b.HasKey("ID");

                    b.HasIndex("YearID");

                    b.ToTable("Wages");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Year", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Years");
                });

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
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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

            modelBuilder.Entity("DTID.BusinessLogic.Models.BalanceOfPayment", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Month", "Month")
                        .WithMany()
                        .HasForeignKey("MonthID");

                    b.HasOne("DTID.BusinessLogic.Models.Quarter", "Quarter")
                        .WithMany()
                        .HasForeignKey("QuarterID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.BoardOfInvestment", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Category", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Indicator", "Indicator")
                        .WithMany("Categories")
                        .HasForeignKey("IndicatorID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Column", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Category", "Category")
                        .WithMany("Columns")
                        .HasForeignKey("CategoryID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.ColumnValues", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Column", "Column")
                        .WithMany("Values")
                        .HasForeignKey("ColumnID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Directory", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Directory", "Parent")
                        .WithMany("Directories")
                        .HasForeignKey("ParentID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.ExchangeRate", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Month", "Month")
                        .WithMany()
                        .HasForeignKey("MonthID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.GrossInternationalReserve", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Month", "Month")
                        .WithMany()
                        .HasForeignKey("MonthID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Indicator", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.SourceFile", "File")
                        .WithMany()
                        .HasForeignKey("FileID");

                    b.HasOne("DTID.BusinessLogic.Models.Directory", "Parent")
                        .WithMany("Indicators")
                        .HasForeignKey("ParentID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Industry", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Quarter", "Quarter")
                        .WithMany()
                        .HasForeignKey("QuarterID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.InflationRate", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Month", "Month")
                        .WithMany()
                        .HasForeignKey("MonthID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Log", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.User", "Users")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Peza", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Population", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.PurchasingManagerIndex", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Industry", "Industry")
                        .WithMany()
                        .HasForeignKey("IndustryID");

                    b.HasOne("DTID.BusinessLogic.Models.Month", "Month")
                        .WithMany()
                        .HasForeignKey("MonthID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.QuarterYear", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Quarter", "Quarter")
                        .WithMany()
                        .HasForeignKey("QuarterID");

                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.SubIndustry", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Industry", "Industry")
                        .WithMany()
                        .HasForeignKey("IndustryID");
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.User", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DTID.BusinessLogic.Models.Wage", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.Year", "Year")
                        .WithMany()
                        .HasForeignKey("YearID")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("DTID.BusinessLogic.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.ApplicationUser")
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

                    b.HasOne("DTID.BusinessLogic.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DTID.BusinessLogic.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
