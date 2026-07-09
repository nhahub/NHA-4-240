using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CodeBook.Data.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CodeBookContext _context;
        public UserRepository(CodeBookContext context) { _context = context; }
        public User GetProfileById(int userid)
        {
            return _context.users.Include(u => u.Posts).Include(u => u.Comments).Include(u => u.Reactions).Include(u => u.Followers).Include(u => u.Reports).Include(u => u.SavedPosts).FirstOrDefault(u => u.Id == userid);
        }
        public User GetProfileByEmail(string email)
        { 
            return _context.users.FirstOrDefault(e => e.Email == email);
        }
        public void Add(User user)
        {
           _context.users.Add(user);
        }

        public void Remove(User user)
        {
            _context.users.Remove(user);
        }
        public void Update(User user)
        {
            _context.users.Update(user);
        }

        public List<User> SearchUsers(string keyword)
        {
            return _context.users
                .Where(u => u.UserName.Contains(keyword) || u.Bio.Contains(keyword))
                .ToList();
        }

        public User FindByUsername(string username)
        {
            return _context.users
                .FirstOrDefault(u => u.UserName == username);
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
