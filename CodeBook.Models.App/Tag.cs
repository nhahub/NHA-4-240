namespace CodeBook.Models.App
{


	public class Tag:BaseEntity
	{
		public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
	}
}
