using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdjustQuestionAnswerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_AnswerOption_AnswerOptionId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTemplates_QuestionTypes_QuestionTypeId",
                table: "QuestionTemplates");

            migrationBuilder.DropIndex(
                name: "IX_QuestionTemplates_QuestionTypeId",
                table: "QuestionTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Answer_AnswerOptionId",
                table: "Answer");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answer",
                type: "varchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionTypeId",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WordCloudAnswer_Text",
                table: "Answer",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerOptionId",
                table: "Answer",
                column: "AnswerOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_AnswerOption_AnswerOptionId",
                table: "Answer",
                column: "AnswerOptionId",
                principalTable: "AnswerOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_AnswerOption_AnswerOptionId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_AnswerOptionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "WordCloudAnswer_Text",
                table: "Answer");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answer",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTemplates_QuestionTypeId",
                table: "QuestionTemplates",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerOptionId",
                table: "Answer",
                column: "AnswerOptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_AnswerOption_AnswerOptionId",
                table: "Answer",
                column: "AnswerOptionId",
                principalTable: "AnswerOption",
                principalColumn: "Id");

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
