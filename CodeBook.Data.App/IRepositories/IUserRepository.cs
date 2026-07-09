using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface IUserRepository
    {
        User GetProfileById(int userid);
        User GetProfileByEmail(string email);
        void Add(User user);
        void Remove(User user);
        void Update(User user); 
        bool SaveChanges();
        List<User> SearchUsers(string keyword);
        User FindByUsername(string username);
    }
}
