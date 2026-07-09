using CodeBook.Models.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBook.Data.App.IRepositories;

namespace CodeBook.Data.App.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CodeBookContext _context;

        public PostRepository(CodeBookContext context) {
            _context = context;
        }

        public Post GetPostById(int postid)
        {
            Post post = _context.posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .FirstOrDefault(p => p.Id == postid);

            if (post == null)
                throw new Exception("Post Not Found!!");

            return post;
         /*   Post post = _context.posts.FirstOrDefault(p => p.Id == postid);
            if (post == null) {
                throw new Exception("Post Not Found!!"); }
            else return post;*/
        }
        public void Add(Post post)
        {
            _context.posts.Add(post);
        }

        public void Update(Post post)
        {
            _context.posts.Update(post);
        }

        public void Delete(Post post)
        {
            _context.posts.Remove(post);
        }

        public List<Post> Getfeed()
        {
            return _context.posts.Include(p=>p.Author).Where(p => p.IsPublic && !p.IsRemoved).ToList();
        }
        public void SavePost(PostSaved saved)
        {
            _context.postsSaved.Add(saved);
        }
        public void AddRemovalRecord(PostRemoval postRemoval)
        {
            _context.postsRemovals.Add(postRemoval);
        }
        public void AddReaction(int postId, ReactionType type,int userId)
        {
            var post = GetPostById(postId);
            if(post != null)
            {
                //post.Reactions.Add(new Reaction {UserId=userId,PostId = postId, Type = type });
                post.LikeCount++;
            }
        }
        public void RemoveReaction(int postId)
        {
            var post = GetPostById(postId);
            if (post != null)
            {
                post.LikeCount--;
            }
        }
        public void AddComment(int postId)
        {
            var post = GetPostById(postId);
            if (post != null)
            {
                post.CommentCount += 1;
            }
        }
        public void RemoveComment(int postId,int count)
        {
            var post = GetPostById(postId);
            if (post != null)
            {
                post.CommentCount = Math.Max(0, post.CommentCount - count);
            }
        }
        public List<PostTag> GetPostTags(int postId)
        {
            return _context.postTags.Where(p => p.PostId == postId).ToList();
        }
        
        public void AddTag(PostTag tag)
        {
            _context.postTags.Add(tag);
        }

        public void RemoveTag(PostTag tag)
        {
            _context.postTags.Remove(tag);
        }

        public PostTag GetPostTagbyId(int postId, int tagId)
        {
            PostTag postTag = _context.postTags.FirstOrDefault(p => p.PostId == postId && p.TagId == tagId);
            if (postTag == null)
                throw new Exception("Tag Not Found");
            return postTag;
        }
        public IQueryable<Post> GetAllUnremoved()
        {
            return _context.posts.Include(p=>p.Author).Include(p=>p.Community).Where(p => p.IsRemoved == false);
        }

        public List<Post> SearchPosts(int userId, string? keyword, string? language, string? tag)
        {
            var query = _context.posts
                .Include(p => p.Author)
                    .ThenInclude(a => a.Followers)
                .Include(p => p.Community)
                    .ThenInclude(c => c.Members)
                .Where(p => !p.IsRemoved);

            query = query.Where(p =>
                p.IsPublic ||
                p.AuthorId == userId ||
                (p.CommunityId != null &&
                 p.Community != null &&
                 p.Community.Members.Any(m => m.UserId == userId)) ||
                p.Author.Followers.Any(f => f.FollowerUserId == userId)
            );

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(p =>
                    p.Title.Contains(keyword) ||
                    p.Body.Contains(keyword));

            if (!string.IsNullOrEmpty(language))
                query = query.Where(p =>
                    p.Language != null &&
                    p.Language == language);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(p =>
                    p.PostTags.Any(t => t.Tag.Name == tag));

            return query.ToList();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool IsPostSavedByUser(int userId, int postId)
        {
            return _context.postsSaved.Any(s => s.UserId == userId && s.PostId == postId);
        }

        public List<Post> GetSavedPosts(int userId)
        {
            return _context.postsSaved
                .Where(ps => ps.UserId == userId)
                .Include(ps => ps.Post)
                    .ThenInclude(p => p.Author)
                .Select(ps => ps.Post)
                .Where(p => !p.IsRemoved)
                .ToList();
        }

        public PostSaved GetSavedPost(int userId, int postId)
        {
            return _context.postsSaved.FirstOrDefault(ps => ps.UserId == userId && ps.PostId == postId);
        }

        public void UnsavePost(int userId, int postId)
        {
            var saved = GetSavedPost(userId, postId);
            if (saved != null)
                _context.postsSaved.Remove(saved);
        }
    } 
}

