
namespace CodeBook.Models.App
{

    public class Reaction : BaseEntity
    {
        public int UserId {get; set;}
        public int? PostId {get; set;}
        public int? CommentId {get; set;}
        public ReactionType Type {get; set;}

        public User User { get; set; } = null!;
        public Post? Post {get; set;}
        public Comment? Comment {get; set;}

    }
}