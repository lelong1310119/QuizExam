using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExamOnline.Migrations
{
    /// <inheritdoc />
    public partial class update_subject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "ENUM",
                table: "AppSubject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                schema: "ENUM",
                table: "AppSubject",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
