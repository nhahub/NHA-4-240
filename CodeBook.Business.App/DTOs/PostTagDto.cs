using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBook.Business.App.DTOs
{
    public class PostTagDto
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
    }
}
