using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My.QuickCampus.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuickCampusId = table.Column<string>(type: "TEXT", nullable: true),
                    ScholarNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_StudentId", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsCurrentClass = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Section = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade_GradeId", x => x.GradeId);
                    table.ForeignKey(
                        name: "FK_Grade_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    AssignmentId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GradeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    QuickCampusId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ClassSectionName = table.Column<string>(type: "TEXT", nullable: true),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: true),
                    PostedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusCreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusEditedBy = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusCreatedDate = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusEditedDate = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedByEmployeeNo = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedByEmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    EditedByEmployeeNo = table.Column<string>(type: "TEXT", nullable: true),
                    EditedByEmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment_AssignmentId", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignment_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Homework",
                columns: table => new
                {
                    HomeworkId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GradeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    QuickCampusId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ClassSectionName = table.Column<string>(type: "TEXT", nullable: true),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: true),
                    PostedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusCreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusEditedBy = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusCreatedDate = table.Column<string>(type: "TEXT", nullable: true),
                    QuickCampusEditedDate = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedByEmployeeNo = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedByEmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    EditedByEmployeeNo = table.Column<string>(type: "TEXT", nullable: true),
                    EditedByEmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homework_HomeworkId", x => x.HomeworkId);
                    table.ForeignKey(
                        name: "FK_Homework_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickCampusSync",
                columns: table => new
                {
                    QuickCampusSyncId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GradeId = table.Column<long>(type: "INTEGER", nullable: false),
                    SourceMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceYear = table.Column<int>(type: "INTEGER", nullable: false),
                    SyncType = table.Column<string>(type: "TEXT", nullable: true),
                    SyncDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickCampusSync_QuickCampusSyncId", x => x.QuickCampusSyncId);
                    table.ForeignKey(
                        name: "FK_QuickCampusSync_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFile",
                columns: table => new
                {
                    MediaFileId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignmentId = table.Column<long>(type: "INTEGER", nullable: true),
                    HomeworkId = table.Column<long>(type: "INTEGER", nullable: true),
                    MediaType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FileExtension = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    QuickCampusFileName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFile_MediaFileId", x => x.MediaFileId);
                    table.ForeignKey(
                        name: "FK_MediaFile_Assignment_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "Assignment",
                        principalColumn: "AssignmentId");
                    table.ForeignKey(
                        name: "FK_MediaFile_Homework_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "Homework",
                        principalColumn: "HomeworkId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_GradeId",
                table: "Assignment",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_StudentId",
                table: "Grade",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Homework_GradeId",
                table: "Homework",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickCampusSync_GradeId",
                table: "QuickCampusSync",
                column: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFile");

            migrationBuilder.DropTable(
                name: "QuickCampusSync");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "Homework");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
