namespace CodeBook.Models.App
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Community> Communities { get; set; } = new List<Community>();
        public ICollection<CommunityMember> CommunityMembership { get; set; } = new List<CommunityMember>();
        public ICollection<PostSaved> SavedPosts { get; set; } = new List<PostSaved>();
        public ICollection<PostRemoval> PostRemovals { get; set; } = new List<PostRemoval>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<CommentRemoval> CommentRemovals { get; set; } = new List<CommentRemoval>();
    }
}
