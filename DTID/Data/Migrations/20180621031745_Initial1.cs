using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DTID.Data.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardOfInvestments_Years_YearID",
                table: "BoardOfInvestments");

            migrationBuilder.DropForeignKey(
                name: "FK_Pezas_Years_YearID",
                table: "Pezas");

            migrationBuilder.RenameColumn(
                name: "YearID",
                table: "Pezas",
                newName: "YearId");

            migrationBuilder.RenameIndex(
                name: "IX_Pezas_YearID",
                table: "Pezas",
                newName: "IX_Pezas_YearId");

            migrationBuilder.RenameColumn(
                name: "YearID",
                table: "BoardOfInvestments",
                newName: "YearId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardOfInvestments_YearID",
                table: "BoardOfInvestments",
                newName: "IX_BoardOfInvestments_YearId");

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "Pezas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "BoardOfInvestments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardOfInvestments_Years_YearId",
                table: "BoardOfInvestments",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pezas_Years_YearId",
                table: "Pezas",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardOfInvestments_Years_YearId",
                table: "BoardOfInvestments");

            migrationBuilder.DropForeignKey(
                name: "FK_Pezas_Years_YearId",
                table: "Pezas");

            migrationBuilder.RenameColumn(
                name: "YearId",
                table: "Pezas",
                newName: "YearID");

            migrationBuilder.RenameIndex(
                name: "IX_Pezas_YearId",
                table: "Pezas",
                newName: "IX_Pezas_YearID");

            migrationBuilder.RenameColumn(
                name: "YearId",
                table: "BoardOfInvestments",
                newName: "YearID");

            migrationBuilder.RenameIndex(
                name: "IX_BoardOfInvestments_YearId",
                table: "BoardOfInvestments",
                newName: "IX_BoardOfInvestments_YearID");

            migrationBuilder.AlterColumn<int>(
                name: "YearID",
                table: "Pezas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "YearID",
                table: "BoardOfInvestments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BoardOfInvestments_Years_YearID",
                table: "BoardOfInvestments",
                column: "YearID",
                principalTable: "Years",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pezas_Years_YearID",
                table: "Pezas",
                column: "YearID",
                principalTable: "Years",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
