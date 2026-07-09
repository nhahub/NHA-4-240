using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBook.Data.App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConstraints : Migration
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
                name: "FK_Posts_Saved_Posts_PostId",
                table: "Posts_Saved");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Saved_Users_UserId",
                table: "Posts_Saved");

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
                name: "FK_Posts_Saved_Posts_PostId",
                table: "Posts_Saved",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Saved_Users_UserId",
                table: "Posts_Saved",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_ID");
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
                name: "FK_Posts_Saved_Posts_PostId",
                table: "Posts_Saved");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Saved_Users_UserId",
                table: "Posts_Saved");

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
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Saved_Posts_PostId",
                table: "Posts_Saved",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Post_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Saved_Users_UserId",
                table: "Posts_Saved",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
