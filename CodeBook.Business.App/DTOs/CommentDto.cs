using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public string Body { get; set; }
        public int LikeCount { get; set; }
        public int? SelfCommentId { get; set; }  // for replies
        public DateTime DateCreated { get; set; }
        public bool isOwner { get; set; } = false;
    }
}
