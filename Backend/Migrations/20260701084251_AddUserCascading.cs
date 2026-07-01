using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCascading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
