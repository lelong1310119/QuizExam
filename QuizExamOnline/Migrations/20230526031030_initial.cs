using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExamOnline.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ENUM");

            migrationBuilder.CreateTable(
                name: "AppGrade",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLevel",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppQuestionGroup",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuestionGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppQuestionType",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRole",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSubject",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Avatar = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DateRefresh = table.Column<DateTime>(type: "datetime", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppExam",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: false),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    Time = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exam_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exam_Grade",
                        column: x => x.GradeId,
                        principalSchema: "ENUM",
                        principalTable: "AppGrade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exam_Level",
                        column: x => x.LevelId,
                        principalSchema: "ENUM",
                        principalTable: "AppLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exam_Status",
                        column: x => x.StatusId,
                        principalSchema: "ENUM",
                        principalTable: "AppStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exam_Subject",
                        column: x => x.SubjectId,
                        principalSchema: "ENUM",
                        principalTable: "AppSubject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: false),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionGroupId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    QuesionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Grade",
                        column: x => x.GradeId,
                        principalSchema: "ENUM",
                        principalTable: "AppGrade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Level",
                        column: x => x.LevelId,
                        principalSchema: "ENUM",
                        principalTable: "AppLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_QuestionGroup",
                        column: x => x.QuestionGroupId,
                        principalSchema: "ENUM",
                        principalTable: "AppQuestionGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_QuestionType",
                        column: x => x.QuesionTypeId,
                        principalSchema: "ENUM",
                        principalTable: "AppQuestionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Status",
                        column: x => x.StatusId,
                        principalSchema: "ENUM",
                        principalTable: "AppStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Subject",
                        column: x => x.SubjectId,
                        principalSchema: "ENUM",
                        principalTable: "AppSubject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppUserRole",
                columns: table => new
                {
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRole", x => new { x.RoleId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_AppUserRole_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppUserRole_Role",
                        column: x => x.RoleId,
                        principalSchema: "ENUM",
                        principalTable: "AppRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppExamHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CompleteTime = table.Column<long>(type: "bigint", nullable: false),
                    QuestionRight = table.Column<double>(type: "float", nullable: false),
                    ToTalScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamHistory_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamHistory_Exam",
                        column: x => x.ExamId,
                        principalTable: "AppExam",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAnswerQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRight = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAnswerQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerQuestion_Question",
                        column: x => x.QuestionId,
                        principalTable: "AppQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppExamQuestion",
                columns: table => new
                {
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    Mark = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamQuestion", x => new { x.ExamId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ExamQuestion_Exam",
                        column: x => x.ExamId,
                        principalTable: "AppExam",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamQuestion_Question",
                        column: x => x.QuestionId,
                        principalTable: "AppQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAnswerQuestion_QuestionId",
                table: "AppAnswerQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExam_AppUserId",
                table: "AppExam",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExam_GradeId",
                table: "AppExam",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExam_LevelId",
                table: "AppExam",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExam_StatusId",
                table: "AppExam",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExam_SubjectId",
                table: "AppExam",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamHistory_AppUserId",
                table: "AppExamHistory",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamHistory_ExamId",
                table: "AppExamHistory",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamQuestion_QuestionId",
                table: "AppExamQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_AppUserId",
                table: "AppQuestion",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_GradeId",
                table: "AppQuestion",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_LevelId",
                table: "AppQuestion",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_QuesionTypeId",
                table: "AppQuestion",
                column: "QuesionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_QuestionGroupId",
                table: "AppQuestion",
                column: "QuestionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_StatusId",
                table: "AppQuestion",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestion_SubjectId",
                table: "AppQuestion",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRole_AppUserId",
                table: "AppUserRole",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAnswerQuestion");

            migrationBuilder.DropTable(
                name: "AppExamHistory");

            migrationBuilder.DropTable(
                name: "AppExamQuestion");

            migrationBuilder.DropTable(
                name: "AppUserRole");

            migrationBuilder.DropTable(
                name: "AppExam");

            migrationBuilder.DropTable(
                name: "AppQuestion");

            migrationBuilder.DropTable(
                name: "AppRole",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "AppGrade",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppLevel",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppQuestionGroup",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppQuestionType",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "AppSubject",
                schema: "ENUM");
        }
    }
}
