
namespace CodeBook.Models.App
{
    public class Notification : BaseEntity
    {
        public NotificationType Type {get; set;}
        public int UserId {get; set;}
        public string Message {get; set;}
        public int ReferenceId {get; set;}
        public bool IsSeen {get; set;}
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public int? SenderId { get; set; }

    }

}