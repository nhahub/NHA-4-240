using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBook.Business.App.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }  //accepted, rejected, pending
        public DateTime DateCreated { get; set; }
    }
}
