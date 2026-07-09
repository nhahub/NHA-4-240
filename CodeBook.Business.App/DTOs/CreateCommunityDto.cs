namespace CodeBook.Business.App.DTOs
{
    public class CreateCommunityDto
    {
        public string? Description { get; set; }
        public string Name { get; set; }=string.Empty;

        public string? IconURL { get; set; } 
    }
}
