namespace CodeBook.Models.App
{
	public class Comment : BaseEntity
	{
		public string Body {get; set;} = string.Empty;
		public int LikeCount {get; set;} = 0;
		public int AuthorId {get; set;}
		public int PostId {get; set;}
		public bool isRemoved { get; set; } = false;
		public DateTime DateUpdated { get; set; }
        public int? SelfCommentId {get; set;} //for reply

		public User Author {get; set;} = null!;
		public Post Post {get; set;} = null!;
		public Comment? selfComment {get; set;}
		public ICollection<Comment> Replies {get; set;} = new List<Comment>();
		public ICollection<Reaction> Reactions {get; set;} = new List<Reaction>();
        public CommentRemoval Removal { get; set; }
    }
}
