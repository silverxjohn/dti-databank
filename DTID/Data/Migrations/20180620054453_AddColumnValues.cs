using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DTID.Data.Migrations
{
    public partial class AddColumnValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColumnValues",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColumnID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
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
                name: "IX_ColumnValues_ColumnID",
                table: "ColumnValues",
                column: "ColumnID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColumnValues");
        }
    }
}
