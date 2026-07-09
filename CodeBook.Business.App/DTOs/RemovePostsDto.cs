using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class RemovePostsDto
    {
        public int? ReportId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
