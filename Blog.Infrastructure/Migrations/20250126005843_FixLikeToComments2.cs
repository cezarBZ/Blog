using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLikeToComments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Comment_CommentId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Post_PostId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_CommentId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_PostId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "CommentId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                schema: "BLOG",
                table: "Comment",
                column: "PostId",
                principalSchema: "BLOG",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId1",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Comment_TargetId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Post_TargetId",
                schema: "BLOG",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PostId1",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "PostId1",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                schema: "BLOG",
                table: "Like",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                schema: "BLOG",
                table: "Like",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Like_CommentId",
                schema: "BLOG",
                table: "Like",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                schema: "BLOG",
                table: "Like",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post",
                schema: "BLOG",
                table: "Comment",
                column: "PostId",
                principalSchema: "BLOG",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Comment_CommentId",
                schema: "BLOG",
                table: "Like",
                column: "CommentId",
                principalSchema: "BLOG",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Post_PostId",
                schema: "BLOG",
                table: "Like",
                column: "PostId",
                principalSchema: "BLOG",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
