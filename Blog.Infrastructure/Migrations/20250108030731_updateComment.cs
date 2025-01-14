using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "BLOG",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                schema: "BLOG",
                table: "Comment",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserId",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_UserId",
                schema: "BLOG",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "BLOG",
                table: "Comment");
        }
    }
}
