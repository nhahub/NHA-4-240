using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Models.App
{
    public class CommentRemoval: BaseEntity

    {
        public int CommentId { get; set; }
        public int RemoverId { get; set; }
        public int? ReportId { get; set; }
        public string Reason { get; set; }

        public Comment Comment { get; set; } = null!;
        public User Remover { get; set; } = null!;
        public Report? Report { get; set; }
    }
}
