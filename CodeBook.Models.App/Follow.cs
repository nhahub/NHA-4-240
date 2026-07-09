namespace CodeBook.Models.App

{
	public class Follow
	{
		public int FollowerUserId {get; set;}
		public int FolloweeUserId {get; set;}
		public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

		public User Follower { get; set; } = null!;
		public User Followee { get; set; } = null!;
	

}
}