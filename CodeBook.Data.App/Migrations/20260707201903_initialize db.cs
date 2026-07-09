using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBook.Data.App.Migrations
{
    /// <inheritdoc />
    public partial class initializedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Tag_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Tag_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(2050)", maxLength: 2050, nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "communities",
                columns: table => new
                {
                    Community_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IconURL = table.Column<string>(type: "nvarchar(2050)", maxLength: 2050, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communities", x => x.Community_ID);
                    table.ForeignKey(
                        name: "FK_communities_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    FollowerUserId = table.Column<int>(type: "int", nullable: false),
                    FolloweeUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follows", x => new { x.FollowerUserId, x.FolloweeUserId });
                    table.ForeignKey(
                        name: "FK_follows_Users_FolloweeUserId",
                        column: x => x.FolloweeUserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follows_Users_FollowerUserId",
                        column: x => x.FollowerUserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Notification_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Notification_ID);
                    table.ForeignKey(
                        name: "FK_notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "communityMembers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communityMembers", x => new { x.UserId, x.CommunityId });
                    table.ForeignKey(
                        name: "FK_communityMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_communityMembers_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "Community_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Post_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    CodeSnippet = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    DateRemoved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Post_ID);
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "Community_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Comment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    isRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelfCommentId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Comment_ID);
                    table.ForeignKey(
                        name: "FK_comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comments_comments_SelfCommentId",
                        column: x => x.SelfCommentId,
                        principalTable: "comments",
                        principalColumn: "Comment_ID");
                });

            migrationBuilder.CreateTable(
                name: "Post_Tags",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Tags", x => new { x.TagId, x.PostId });
                    table.ForeignKey(
                        name: "FK_Post_Tags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Tags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Tag_ID");
                });

            migrationBuilder.CreateTable(
                name: "Posts_Saved",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts_Saved", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_Posts_Saved_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Saved_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Reaction_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Reaction_ID);
                    table.ForeignKey(
                        name: "FK_Reactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Comment_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Report_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Report_ID);
                    table.ForeignKey(
                        name: "FK_Reports_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID");
                    table.ForeignKey(
                        name: "FK_Reports_Users_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                    table.ForeignKey(
                        name: "FK_Reports_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Comment_ID");
                });

            migrationBuilder.CreateTable(
                name: "Comment_Removal",
                columns: table => new
                {
                    Removal_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    RemoverId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date_Removed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment_Removal", x => x.Removal_ID);
                    table.ForeignKey(
                        name: "FK_Comment_Removal_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Report_ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comment_Removal_Users_RemoverId",
                        column: x => x.RemoverId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                    table.ForeignKey(
                        name: "FK_Comment_Removal_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Comment_ID");
                });

            migrationBuilder.CreateTable(
                name: "Post_Removal",
                columns: table => new
                {
                    Removal_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    RemoverId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date_Removed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Removal", x => x.Removal_ID);
                    table.ForeignKey(
                        name: "FK_Post_Removal_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Post_ID");
                    table.ForeignKey(
                        name: "FK_Post_Removal_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Report_ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Post_Removal_Users_RemoverId",
                        column: x => x.RemoverId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Removal_CommentId",
                table: "Comment_Removal",
                column: "CommentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Removal_RemoverId",
                table: "Comment_Removal",
                column: "RemoverId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Removal_ReportId",
                table: "Comment_Removal",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_AuthorId",
                table: "comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_PostId",
                table: "comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_SelfCommentId",
                table: "comments",
                column: "SelfCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_communities_OwnerId",
                table: "communities",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_communityMembers_CommunityId",
                table: "communityMembers",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_follows_FolloweeUserId",
                table: "follows",
                column: "FolloweeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId",
                table: "notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Removal_PostId",
                table: "Post_Removal",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_Removal_RemoverId",
                table: "Post_Removal",
                column: "RemoverId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Removal_ReportId",
                table: "Post_Removal",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Tags_PostId",
                table: "Post_Tags",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommunityId",
                table: "Posts",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Saved_PostId",
                table: "Posts_Saved",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CommentId",
                table: "Reports",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment_Removal");

            migrationBuilder.DropTable(
                name: "communityMembers");

            migrationBuilder.DropTable(
                name: "follows");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "Post_Removal");

            migrationBuilder.DropTable(
                name: "Post_Tags");

            migrationBuilder.DropTable(
                name: "Posts_Saved");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "communities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
