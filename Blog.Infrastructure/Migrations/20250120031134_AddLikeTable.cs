using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLikeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserId",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "BLOG",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                schema: "BLOG",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "BLOG",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "BLOG",
                table: "Comment",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Like",
                schema: "BLOG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Like_Post",
                        column: x => x.PostId,
                        principalSchema: "BLOG",
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Likes",
                        column: x => x.UserId,
                        principalSchema: "BLOG",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                schema: "BLOG",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                schema: "BLOG",
                table: "Like",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_UserId",
                schema: "BLOG",
                table: "Like",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Comments",
                schema: "BLOG",
                table: "Comment",
                column: "UserId",
                principalSchema: "BLOG",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserId",
                schema: "BLOG",
                table: "Post",
                column: "UserId",
                principalSchema: "BLOG",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Comments",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserId",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.DropTable(
                name: "Like",
                schema: "BLOG");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserId",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "BLOG",
                table: "Comment",
                type: "nvarchar(1000)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_UserId",
                schema: "BLOG",
                table: "Comment",
                column: "UserId",
                principalSchema: "BLOG",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
