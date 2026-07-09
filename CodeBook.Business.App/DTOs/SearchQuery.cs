using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBook.Business.App.DTOs
{
    public class SearchQuery
    {
        public string Keyword { get; set; }
        public string Language { get; set; }
        public string Tag { get; set; }
        public int CommunityId { get; set; }
    }
}
