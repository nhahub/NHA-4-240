using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class CreatePostRequest
    {
 
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPublic { get; set; }
        public int? CommunityId { get; set; }
        public string? CodeSnippet { get; set; }
        public string? Language { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
