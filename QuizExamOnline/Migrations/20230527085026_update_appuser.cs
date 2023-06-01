using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExamOnline.Migrations
{
    /// <inheritdoc />
    public partial class update_appuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRefresh",
                table: "AppUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRefresh",
                table: "AppUser",
                type: "datetime",
                nullable: true);
        }
    }
}
