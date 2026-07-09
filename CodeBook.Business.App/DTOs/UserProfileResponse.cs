using CodeBook.Models.App;
using System;

namespace CodeBook.Business.App.DTOs
{
    public class UserProfileResponse
    {
        public int Id {  get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime JoinedAt { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public UserRole Role { get; set; }
    }
}