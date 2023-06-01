using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExamOnline.Migrations
{
    /// <inheritdoc />
    public partial class updatequestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuesionTypeId",
                table: "AppQuestion",
                newName: "QuestionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AppQuestion_QuesionTypeId",
                table: "AppQuestion",
                newName: "IX_AppQuestion_QuestionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionTypeId",
                table: "AppQuestion",
                newName: "QuesionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AppQuestion_QuestionTypeId",
                table: "AppQuestion",
                newName: "IX_AppQuestion_QuesionTypeId");
        }
    }
}
