namespace CodeBook.Models.App

{
	public class Community : BaseEntity
	{
		public int? OwnerId {get; set;}
		public string Name {get; set;} = string.Empty;
		public string? Description {get; set;}
		public string? IconURL {get; set;}
		public string Slug {get; set;} = string.Empty;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public User? Owner {get; set;} = null!;
		public ICollection<Post> Posts {get; set;} = new List<Post>();
		public ICollection<CommunityMember> Members {get; set;} = new List<CommunityMember>();

	}
}