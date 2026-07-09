using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface IFollowRepository
    {
        void AddFollow(Follow follow);
        void RemoveFollow(Follow follow);
        Follow GetFollow(int followerId, int followeeId);
        int GetFollowersCount(int userId);
        int GetFollowingCount(int userId);
         List<User> GetFollowers(int userId);
         List<User> GetFollowing(int userId);
        bool SaveChanges();
    }

}
