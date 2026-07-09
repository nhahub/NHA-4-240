namespace CodeBook.Models.App

{
    public abstract class BaseEntity
    {
        public int Id {get; set;}
        public DateTime DateCreated {get; set;} = DateTime.UtcNow;
    }
}