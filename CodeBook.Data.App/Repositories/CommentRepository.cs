using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CodeBook.Data.App.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CodeBookContext _context;
        public CommentRepository(CodeBookContext context) { _context = context; }
        public Comment GetCommentById(int commentid)
        {
            Comment comment = _context.comments.FirstOrDefault(c => c.Id == commentid);
            if (comment == null)
                throw new Exception("Comment Not Found!!");
            return comment;
        }
        public void Add(Comment comment)
        {
            _context.comments.Add(comment);
        }
        public void Update(Comment comment)
        {
            _context.comments.Update(comment);
        }
        public int Delete(Comment comment)
        {
            int deletedCount = DeleteRepliesRecursively(comment.Id);

            var reactions = _context.reactions.Where(r => r.CommentId == comment.Id);
            _context.reactions.RemoveRange(reactions);
            _context.comments.Remove(comment);
            return deletedCount + 1;
        }
        private int DeleteRepliesRecursively(int commentId) {
            int count = 0; ;
            var replies = _context.comments.Where(c => c.SelfCommentId == commentId).ToList();

            foreach (var reply in replies) {
                count+=DeleteRepliesRecursively(reply.Id);
                var replyReactions = _context.reactions
                   .Where(r => r.CommentId == reply.Id);
                _context.reactions.RemoveRange(replyReactions);
                _context.comments.Remove(reply);
                count++;
            }
            return count;
        }

        public List<Comment> GetByPostId(int postId)
        {
            return _context.comments.Where(c => c.PostId == postId).Include(c => c.Author).Include(c => c.Replies).ToList();
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
        public void AddRemovalRecord(CommentRemoval removal)
        {
            _context.commentsRemovals.Add(removal);
        }
        public void AddReaction(int commentId) {
            Comment c = GetCommentById(commentId);
            if (c != null)
            {
                c.LikeCount += 1;
            }
        }
        public void RemoveReaction(int commentId) {
            Comment c = GetCommentById(commentId);
            if (c != null)
            {
                c.LikeCount -= 1;
            }
        }

    }
}
