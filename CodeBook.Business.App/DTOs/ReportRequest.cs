using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBook.Business.App.DTOs
{
    public class ReportRequest
    {
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
    }
}
