using Microsoft.EntityFrameworkCore;
using CodeBook.Models.App;


namespace CodeBook.Data.App
{
    public class CodeBookContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Tag> tags {  get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Reaction> reactions { get; set; }
        public DbSet<PostTag> postTags { get; set; }
        public DbSet<PostSaved> postsSaved { get; set; }
        public DbSet<PostRemoval> postsRemovals { get; set; }
        public DbSet<CommentRemoval> commentsRemovals { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Follow> follows { get; set; }
        public DbSet<CommunityMember> communityMembers { get; set; }
        public DbSet<Community> communities { get; set; }
        public DbSet<Comment> comments { get; set; }

        public CodeBookContext(DbContextOptions<CodeBookContext> options) : base(options)
        {
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                // Configure the database connection string here
                optionsBuilder.UseSqlServer("Server=.;Database=CodeBook_DB;Trusted_Connection=true;TrustServerCertificate=true");
            }
            base.OnConfiguring(optionsBuilder);
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("Users");
                user.HasKey(u => u.Id);
                user.Property(u => u.Id).HasColumnName("User_ID").ValueGeneratedOnAdd();
                user.Property(u => u.UserName).IsRequired().HasMaxLength(50);
                user.Property(u => u.Email).IsRequired().HasMaxLength(250);
                user.HasIndex(u => u.Email).IsUnique();
                user.Property(u => u.PasswordHash).IsRequired().HasMaxLength(250);
                user.Property(u => u.Bio).HasMaxLength(500);
                user.Property(u => u.AvatarUrl).HasMaxLength(2050);
                user.Property(u => u.Role).HasMaxLength(30);
            });

            modelBuilder.Entity<Tag>(Tag =>
            {
                Tag.HasKey(t => t.Id);
                Tag.Property(t => t.Id).HasColumnName("Tag_ID").ValueGeneratedOnAdd();
                Tag.Property(t => t.Name).HasMaxLength(50);
                Tag.Property(t => t.Slug).HasMaxLength(255);

            });

            modelBuilder.Entity<Report>(report =>
            {
                report.ToTable("Reports");
                report.HasKey(r => r.Id);
                report.Property(r => r.Id).HasColumnName("Report_ID").ValueGeneratedOnAdd();
                report.Property(r => r.Reason).HasMaxLength(500);
                report.Property(r => r.Description).HasMaxLength(1000);
                report.Property(r => r.Status).HasMaxLength(50);
                report.HasOne(r => r.Reporter).WithMany(u => u.Reports).HasForeignKey(r => r.ReporterId).OnDelete(DeleteBehavior.NoAction);
                report.HasOne(r => r.Post).WithMany(p => p.Reports).HasForeignKey(r => r.PostId).OnDelete(DeleteBehavior.SetNull);
                report.HasOne(r => r.Comment).WithMany().HasForeignKey(r => r.CommentId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Reaction>(reaction =>
            {
                reaction.ToTable("Reactions");
                reaction.HasKey(r => r.Id);
                reaction.Property(r => r.Id).HasColumnName("Reaction_ID").ValueGeneratedOnAdd();
                reaction.Property(r => r.Type).HasMaxLength(30);
                reaction.HasOne(r => r.User).WithMany(u => u.Reactions).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);
                reaction.HasOne(r => r.Post).WithMany(p => p.Reactions).HasForeignKey(r => r.PostId).OnDelete(DeleteBehavior.Cascade);
                reaction.HasOne(r => r.Comment).WithMany(c => c.Reactions).HasForeignKey(r => r.CommentId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PostTag>(postTag =>
            {
                postTag.ToTable("Post_Tags");
                postTag.HasKey(pt => new { pt.TagId, pt.PostId });
                postTag.HasOne(pt => pt.Post).WithMany(p => p.PostTags).HasForeignKey(pt => pt.PostId).OnDelete(DeleteBehavior.Cascade);
                postTag.HasOne(pt => pt.Tag).WithMany(t => t.PostTags).HasForeignKey(pt => pt.TagId).OnDelete(DeleteBehavior.NoAction);
  
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.ToTable("Posts");
                post.HasKey(post => post.Id);
                post.Property(post => post.Id).HasColumnName("Post_ID").ValueGeneratedOnAdd();
                post.Property(post => post.Title).HasMaxLength(200);
                post.Property(post => post.Body).HasMaxLength(10000);
                post.Property(post => post.CodeSnippet).HasMaxLength(10000);
                post.Property(post => post.Language).HasMaxLength(30);
                post.Property(post => post.IsRemoved).HasDefaultValue(false);
                post.HasOne(p => p.Author).WithMany(u => u.Posts).HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.Cascade);
                post.HasOne(p => p.Community).WithMany(c => c.Posts).HasForeignKey(p => p.CommunityId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PostSaved>(postsaved =>
            {
                postsaved.ToTable("Posts_Saved");
                //Here Composite primary key
                postsaved.HasKey(ps => new { ps.UserId, ps.PostId });
                postsaved.HasOne(ps => ps.User).WithMany(u => u.SavedPosts).HasForeignKey(ps => ps.UserId).OnDelete(DeleteBehavior.NoAction);
                postsaved.HasOne(ps => ps.Post).WithMany(p => p.SavedByUsers).HasForeignKey(ps => ps.PostId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostRemoval>(postremoval =>
            {
                postremoval.ToTable("Post_Removal");
                postremoval.Property(pr => pr.Id).HasColumnName("Removal_ID").ValueGeneratedOnAdd();
                postremoval.Property(pr => pr.Reason).HasMaxLength(1000);
                postremoval.Property(pr => pr.DateCreated).HasColumnName("Date_Removed");
                postremoval.HasOne(pr => pr.Post).WithOne(p => p.Removal).HasForeignKey<PostRemoval>(pr => pr.PostId).OnDelete(DeleteBehavior.NoAction);
                postremoval.HasOne(pr => pr.Remover).WithMany(u => u.PostRemovals).HasForeignKey(pr => pr.RemoverId).OnDelete(DeleteBehavior.NoAction);
                postremoval.HasOne(pr => pr.Report).WithMany().HasForeignKey(pr => pr.ReportId).OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<CommentRemoval>(postremoval =>
            {
                postremoval.ToTable("Comment_Removal");
                postremoval.Property(cr => cr.Id).HasColumnName("Removal_ID").ValueGeneratedOnAdd();
                postremoval.Property(cr => cr.Reason).HasMaxLength(1000);
                postremoval.Property(cr => cr.DateCreated).HasColumnName("Date_Removed");
                postremoval.HasOne(cr => cr.Comment).WithOne(c => c.Removal).HasForeignKey<CommentRemoval>(cr => cr.CommentId).OnDelete(DeleteBehavior.NoAction);
                postremoval.HasOne(cr => cr.Remover).WithMany(u => u.CommentRemovals).HasForeignKey(cr => cr.RemoverId).OnDelete(DeleteBehavior.NoAction);
                postremoval.HasOne(cr => cr.Report).WithMany().HasForeignKey(cr => cr.ReportId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Notification>(notification =>
            {
                notification.Property(n => n.Id).HasColumnName("Notification_ID").ValueGeneratedOnAdd();
                notification.Property(n => n.Type).HasMaxLength(30);
                notification.Property(n => n.Message).HasMaxLength(300);
                notification.Property(n => n.IsSeen).HasDefaultValue(false);
                notification.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Follow>(follow =>
            {
                follow.HasKey(f => new { f.FollowerUserId, f.FolloweeUserId });
                follow.HasOne(f => f.Follower).WithMany(u => u.Following).HasForeignKey(f => f.FollowerUserId).OnDelete(DeleteBehavior.Cascade);
                follow.HasOne(f => f.Followee).WithMany(u => u.Followers).HasForeignKey(f => f.FolloweeUserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Community>(community =>
            {
                community.Property(c => c.Id).HasColumnName("Community_ID").ValueGeneratedOnAdd();
                community.Property(c => c.Name).HasMaxLength(50);
                community.Property(c => c.Description).HasMaxLength(1000);
                community.Property(c => c.IconURL).HasMaxLength(2050);
                community.Property(c => c.Slug).HasMaxLength(255);
                community.HasOne(c => c.Owner).WithMany(o => o.Communities).HasForeignKey(c => c.OwnerId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CommunityMember>(member =>
            {
                member.HasKey(m => new { m.UserId, m.CommunityId });
                member.HasOne(m => m.User).WithMany(u => u.CommunityMembership).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
                member.HasOne(m => m.Community).WithMany(c => c.Members).HasForeignKey(m => m.CommunityId).OnDelete(DeleteBehavior.Cascade);
                member.Property(m => m.Role).HasMaxLength(50);

            });

            modelBuilder.Entity<Comment>(comment =>
            {
                comment.Property(c => c.Id).HasColumnName("Comment_ID").ValueGeneratedOnAdd();
                comment.Property(c => c.Body).HasMaxLength(5000).IsRequired();
                comment.HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction);
                comment.HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);
                comment.HasOne(c => c.selfComment).WithMany(c => c.Replies).HasForeignKey(c => c.SelfCommentId).OnDelete(DeleteBehavior.NoAction);

            });

                base.OnModelCreating(modelBuilder);
        }

    }
}
