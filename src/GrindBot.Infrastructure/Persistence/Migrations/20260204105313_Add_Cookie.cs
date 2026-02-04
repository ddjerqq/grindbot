using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrindBot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Cookie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CookieSentAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Cookies",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookieSentAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cookies",
                table: "Users");
        }
    }
}
