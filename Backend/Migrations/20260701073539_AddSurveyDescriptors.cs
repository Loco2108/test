using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyDescriptors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTemplates_QuestionTypes_QuestionTypeId",
                table: "QuestionTemplates");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropIndex(
                name: "IX_QuestionTemplates_QuestionTypeId",
                table: "QuestionTemplates");

            migrationBuilder.RenameColumn(
                name: "QuestionTypeId",
                table: "QuestionTemplates",
                newName: "QuestionType");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Surveys",
                type: "varchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Surveys",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Surveys");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "QuestionTemplates",
                newName: "QuestionTypeId");

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
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
                name: "IX_QuestionTemplates_QuestionTypeId",
                table: "QuestionTemplates",
                column: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTemplates_QuestionTypes_QuestionTypeId",
                table: "QuestionTemplates",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
