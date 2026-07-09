using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly CodeBookContext _context;
        public FollowRepository(CodeBookContext context) { _context = context; }
        public void AddFollow(Follow follow)
        {
            _context.follows.Add(follow);
        }
        public void RemoveFollow(Follow follow)
        {
            _context.follows.Remove(follow);
        }

        public Follow GetFollow(int followerId, int followeeId)
        {
            return _context.follows.FirstOrDefault(f => f.FollowerUserId == followerId && f.FolloweeUserId == followeeId);
        }
        public int GetFollowersCount(int userId)
        {
            return _context.follows.Count(f => f.FolloweeUserId == userId);
        }
        public int GetFollowingCount(int userId)
        {

            return _context.follows.Count(f => f.FollowerUserId == userId);
        }
        public List<User> GetFollowers(int userId)
        {
            return _context.follows.Where(f=> f.FolloweeUserId==userId).Select(f=>f.Follower).ToList();
        }
        public List<User> GetFollowing(int userId)
        {
            return _context.follows.Where(f => f.FollowerUserId == userId).Select(f => f.Followee).ToList();
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
