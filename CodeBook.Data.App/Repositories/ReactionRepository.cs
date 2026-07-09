using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly CodeBookContext _context;
        public ReactionRepository(CodeBookContext context)
        {
            _context = context;

        }
        public Reaction GetReaction(int postId, int userId)
        {
            return _context.reactions.FirstOrDefault(r => r.PostId == postId && r.UserId == userId);
        }
        public Reaction GetCommentReaction(int postId, int userId,int commentId)
        {
            return _context.reactions.FirstOrDefault(r => r.PostId == postId && r.UserId == userId && r.CommentId == commentId);
        }
        public void Add(Reaction reaction)
        {
            _context.reactions.Add(reaction);
        }

        public void Remove(Reaction reaction)
        {
            _context.reactions.Remove(reaction);
        }

        public void Update(Reaction reaction)
        {
            _context.reactions.Update(reaction);
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
