using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface IReactionRepository
    {
        Reaction GetReaction(int postId, int userId);
        Reaction GetCommentReaction(int postId, int userId,int commentId);
        void Add(Reaction reaction);
        void Remove(Reaction reaction);
        void Update(Reaction reaction);
        bool SaveChanges();
    }
}
