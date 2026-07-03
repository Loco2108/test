using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class LiveSessionAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnonymousUser_AnonymousProfilePicture_ProfilePictureId",
                table: "AnonymousUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_AnonymousUser_ProfilePictureId",
                table: "AnonymousUser");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "AnonymousUser");

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RoomActive",
                table: "Sessions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RoomCode",
                table: "Sessions",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentConnectionId",
                table: "AnonymousUser",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AnonymousUserId",
                table: "AnonymousProfilePicture",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousProfilePicture_AnonymousUserId",
                table: "AnonymousProfilePicture",
                column: "AnonymousUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnonymousProfilePicture_AnonymousUser_AnonymousUserId",
                table: "AnonymousProfilePicture",
                column: "AnonymousUserId",
                principalTable: "AnonymousUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AnonymousProfilePicture_AnonymousUser_AnonymousUserId",
                table: "AnonymousProfilePicture");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_AnonymousProfilePicture_AnonymousUserId",
                table: "AnonymousProfilePicture");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RoomActive",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RoomCode",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CurrentConnectionId",
                table: "AnonymousUser");

            migrationBuilder.DropColumn(
                name: "AnonymousUserId",
                table: "AnonymousProfilePicture");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfilePictureId",
                table: "AnonymousUser",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousUser_ProfilePictureId",
                table: "AnonymousUser",
                column: "ProfilePictureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnonymousUser_AnonymousProfilePicture_ProfilePictureId",
                table: "AnonymousUser",
                column: "ProfilePictureId",
                principalTable: "AnonymousProfilePicture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Folders_FolderId",
                table: "Surveys",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
