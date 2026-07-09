using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBook.Data.App.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Posts_PostId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_Users_AuthorId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Posts_PostId",
                table: "comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Users_AuthorId",
                table: "comments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Posts_PostId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_Users_AuthorId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Posts_PostId",
                table: "comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Users_AuthorId",
                table: "comments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID");
        }
    }
}
