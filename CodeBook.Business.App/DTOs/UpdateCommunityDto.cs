using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class UpdateCommunityDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconURL { get; set; }
    }
}
