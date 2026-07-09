using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBook.Business.App.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int? SenderId { get; set; }

        public string Type { get; set; }
        public string Message { get; set; }
        public int ReferenceId { get; set; }
        public bool IsSeen { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
