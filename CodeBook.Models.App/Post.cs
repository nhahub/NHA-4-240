namespace CodeBook.Models.App
{
	public class Post: BaseEntity
	{
		public string Title {get; set;}=string.Empty;
		public string Body {get; set;}= string.Empty;
		public string? CodeSnippet {get; set;}
		public string? Language {get; set;}
		public int LikeCount {get; set;} = 0;
		public int CommentCount {get; set;} = 0;
		public bool IsRemoved {get; set;} = false;
		public bool IsPublic { get; set; } = false;
		public DateTime? DateRemoved { get; set; } = DateTime.UtcNow;
		public int AuthorId {get; set;}
		public int? CommunityId {get; set;}
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public User Author {get; set;} = null!;
		public Community? Community {get; set;}
		public ICollection<Comment> Comments {get; set;} = new List<Comment>();
		public ICollection<Reaction> Reactions {get; set;} = new List<Reaction>();
		public ICollection<PostTag> PostTags {get; set;} = new List<PostTag>();
		public ICollection<PostSaved> SavedByUsers {get; set;} = new List<PostSaved>();
		public ICollection<Report> Reports {get; set;} = new List<Report>();
		public PostRemoval Removal { get; set; }
    }
}