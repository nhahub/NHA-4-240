
namespace CodeBook.Models.App

{
	public class CommunityMember
	{
        public int UserId {get; set;}
        public int CommunityId {get; set;}
		public CommunityRole Role {get; set;} = CommunityRole.Member;
		public DateTime JoinedAt {get; set;} = DateTime.UtcNow;

		public User User {get; set;} = null!;
		public Community Community {get; set;} = null!;
	}
}
