using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class CommunityDto
    {
        public int communityId {  get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string? IconURL { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int memberscount { get; set; } = 0;
        public bool isOwner { get; set; } = false;
    }
}
