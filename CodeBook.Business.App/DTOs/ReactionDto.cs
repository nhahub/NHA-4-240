using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class ReactionDto
    {
        public int PostId { get; set; }
        public int? CommentId { get; set; }
        public string ReactionType { get; set; }
    }
}
