namespace CodeBook.Models.App
{

	public class PostRemoval:BaseEntity

	{
		public int PostId {get; set;}
		public int RemoverId {get; set;}
		public int? ReportId {get; set;}
		public string Reason {get; set;}

		public Post Post { get; set; } = null!;
		public User Remover { get; set; } = null!;
		public Report? Report {get; set;}
	
	}
}