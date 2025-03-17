using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePostRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "BLOG",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_CreatedBy",
                schema: "BLOG",
                table: "Post",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User",
                schema: "BLOG",
                table: "Post",
                column: "CreatedBy",
                principalSchema: "BLOG",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_CreatedBy",
                schema: "BLOG",
                table: "Post");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "BLOG",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
