using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DTID.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Directories_Directories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Directories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Quarters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Indicators",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Indicators_Directories_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Directories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Contact = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BalanceOfPayments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BalanceOfPayments = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    MonthID = table.Column<int>(nullable: true),
                    QuarterID = table.Column<int>(nullable: true),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceOfPayments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BalanceOfPayments_Months_MonthID",
                        column: x => x.MonthID,
                        principalTable: "Months",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceOfPayments_Quarters_QuarterID",
                        column: x => x.QuarterID,
                        principalTable: "Quarters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceOfPayments_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardOfInvestments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardOfInvestments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BoardOfInvestments_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    MonthID = table.Column<int>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Months_MonthID",
                        column: x => x.MonthID,
                        principalTable: "Months",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrossInternationalReserves",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    MonthID = table.Column<int>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrossInternationalReserves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GrossInternationalReserves_Months_MonthID",
                        column: x => x.MonthID,
                        principalTable: "Months",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GrossInternationalReserves_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InflationRates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    MonthID = table.Column<int>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InflationRates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InflationRates_Months_MonthID",
                        column: x => x.MonthID,
                        principalTable: "Months",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InflationRates_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pezas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pezas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pezas_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Populations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Populations = table.Column<int>(nullable: false),
                    YearId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Populations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Populations_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuarterYears",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuarterID = table.Column<int>(nullable: true),
                    YearID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterYears", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuarterYears_Quarters_QuarterID",
                        column: x => x.QuarterID,
                        principalTable: "Quarters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuarterYears_Years_YearID",
                        column: x => x.YearID,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Wages = table.Column<double>(nullable: false),
                    YearId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Wages_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IndicatorID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Columns_Indicators_IndicatorID",
                        column: x => x.IndicatorID,
                        principalTable: "Indicators",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ColumnValues",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColumnID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    RowId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ColumnValues_Columns_ColumnID",
                        column: x => x.ColumnID,
                        principalTable: "Columns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceOfPayments_MonthID",
                table: "BalanceOfPayments",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceOfPayments_QuarterID",
                table: "BalanceOfPayments",
                column: "QuarterID");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceOfPayments_YearID",
                table: "BalanceOfPayments",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardOfInvestments_YearID",
                table: "BoardOfInvestments",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_IndicatorID",
                table: "Columns",
                column: "IndicatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnValues_ColumnID",
                table: "ColumnValues",
                column: "ColumnID");

            migrationBuilder.CreateIndex(
                name: "IX_Directories_ParentId",
                table: "Directories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_MonthID",
                table: "ExchangeRates",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_YearID",
                table: "ExchangeRates",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_GrossInternationalReserves_MonthID",
                table: "GrossInternationalReserves",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_GrossInternationalReserves_YearID",
                table: "GrossInternationalReserves",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Indicators_ParentID",
                table: "Indicators",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_InflationRates_MonthID",
                table: "InflationRates",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_InflationRates_YearID",
                table: "InflationRates",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Pezas_YearID",
                table: "Pezas",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Populations_YearId",
                table: "Populations",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_QuarterID",
                table: "QuarterYears",
                column: "QuarterID");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_YearID",
                table: "QuarterYears",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wages_YearId",
                table: "Wages",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BalanceOfPayments");

            migrationBuilder.DropTable(
                name: "BoardOfInvestments");

            migrationBuilder.DropTable(
                name: "ColumnValues");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "GrossInternationalReserves");

            migrationBuilder.DropTable(
                name: "InflationRates");

            migrationBuilder.DropTable(
                name: "Pezas");

            migrationBuilder.DropTable(
                name: "Populations");

            migrationBuilder.DropTable(
                name: "QuarterYears");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wages");

            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropTable(
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Years");

            migrationBuilder.DropTable(
                name: "Indicators");

            migrationBuilder.DropTable(
                name: "Directories");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }
    }
}
