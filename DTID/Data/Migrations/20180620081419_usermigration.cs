using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DTID.Data.Migrations
{
    public partial class usermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_RoleID",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "User",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "User",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "User",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleID",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "User",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_RoleId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "User",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "User",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "User",
                newName: "Firstname");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "User",
                newName: "IX_User_RoleID");

            migrationBuilder.AlterColumn<int>(
                name: "RoleID",
                table: "User",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
