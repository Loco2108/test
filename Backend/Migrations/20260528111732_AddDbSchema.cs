using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnonymousProfilePicture",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Body = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Face = table.Column<int>(type: "int", nullable: false),
                    Hat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousProfilePicture", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    OwnerId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FolderId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Surveys_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuestionTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false),
                    SurveyId = table.Column<Guid>(type: "char(36)", nullable: false),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTemplates_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTemplates_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false),
                    SurveyId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnswerOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    QuestionTemplateId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOption_QuestionTemplates_QuestionTemplateId",
                        column: x => x.QuestionTemplateId,
                        principalTable: "QuestionTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnonymousUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProfilePictureId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnonymousUser_AnonymousProfilePicture_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "AnonymousProfilePicture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnonymousUser_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    QuestionTemplateId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_QuestionTemplates_QuestionTemplateId",
                        column: x => x.QuestionTemplateId,
                        principalTable: "QuestionTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: true),
                    AnswerOptionId = table.Column<Guid>(type: "char(36)", nullable: true),
                    AnonymousUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AnonymousUser_AnonymousUserId",
                        column: x => x.AnonymousUserId,
                        principalTable: "AnonymousUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_AnswerOption_AnswerOptionId",
                        column: x => x.AnswerOptionId,
                        principalTable: "AnswerOption",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Choose one of multiple answer options", "Single Choice" },
                    { 2, "Choose multiple answer options", "Multiple Choice" },
                    { 3, "Submit a single word which is displayed in a cloud", "Word Cloud" },
                    { 4, "Submit any text of your choice", "Free Text" },
                    { 5, "Submit a rating from 1 to 10", "Number Scale" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousUser_ProfilePictureId",
                table: "AnonymousUser",
                column: "ProfilePictureId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousUser_SessionId",
                table: "AnonymousUser",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnonymousUserId",
                table: "Answer",
                column: "AnonymousUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerOptionId",
                table: "Answer",
                column: "AnswerOptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOption_QuestionTemplateId",
                table: "AnswerOption",
                column: "QuestionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId",
                table: "Folders",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionTemplateId",
                table: "Question",
                column: "QuestionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SessionId",
                table: "Question",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTemplates_QuestionTypeId",
                table: "QuestionTemplates",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTemplates_SurveyId",
                table: "QuestionTemplates",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SurveyId",
                table: "Sessions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_FolderId",
                table: "Surveys",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_OwnerId",
                table: "Surveys",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "AnonymousUser");

            migrationBuilder.DropTable(
                name: "AnswerOption");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "AnonymousProfilePicture");

            migrationBuilder.DropTable(
                name: "QuestionTemplates");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Folders");
        }
    }
}
