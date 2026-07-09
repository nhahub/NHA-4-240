using AutoMapper;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Middleware;
using CodeBook.Data.App;
using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;

namespace CodeBook.Business.App.Services
{
    public class PostsService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly CodeBookContext _context;


        public PostsService(IPostRepository postRepository, IMapper mapper, CodeBookContext context, ICommentRepository commentRepository, IReactionRepository reactionRepository)
        {
            this._postRepository = postRepository;
            this.mapper = mapper;
            _context = context;
            _commentRepository = commentRepository;
            _reactionRepository = reactionRepository;
        }
        public ErrorResponse CreatePost(int userId,CreatePostRequest request, int? communityId) 
        {
            if(communityId != null)
            {
                bool isMember = _context.communityMembers.Any(m => m.CommunityId == request.CommunityId && m.UserId == userId);
                if (!isMember)
                {
                    return new ErrorResponse { Success = false, Message = "You are not a member of this community" };
                }
            }
            Post post = new Post
            {
                AuthorId = userId,
                Title = request.Title,
                Body = request.Body,
                IsPublic = request.IsPublic,
                CommunityId = communityId,
                CodeSnippet = request.CodeSnippet,
                Language = request.Language,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            _postRepository.Add(post);
            _postRepository.SaveChanges();

            if(request.TagIds != null && request.TagIds.Any())
            {
                foreach(var tagId in request.TagIds)
                {
                    bool tagExists = _context.tags.Any(t => t.Id == tagId);
                    if (tagExists)
                    {
                        PostTag postTag = new PostTag
                        {
                            PostId = post.Id,
                            TagId = tagId
                        };
                        _postRepository.AddTag(postTag);
                    }
                
                }
                _postRepository.SaveChanges();
            }
            return new ErrorResponse { Success = true, Message = "Post created successfully" };
        }
        public ErrorResponse UpdatePost(int postId, UpdatePostRequest request,int userId) 
        {
            Post post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return new ErrorResponse { Success = false, Message = "Post not found" };
            }    
            if (post.AuthorId != userId)
                return new ErrorResponse { Success = false, Message = "You can only edit your own posts" };

            post.Title = request.Title == "string" ? post.Title : request.Title;
            post.Body = request.Body == "string" ? post.Body : request.Body;
            post.IsPublic = request.IsPublic;
            post.CodeSnippet = request.CodeSnippet == "string" ? post.CodeSnippet : request.CodeSnippet;
            post.Language = request.Language == "string" ? post.Language : request.Language;
            post.DateUpdated = DateTime.UtcNow;
            if (_postRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Post updated successfully" };

            return new ErrorResponse { Success = false, Message = "Could not update post" };

        }
        public ErrorResponse DeletePost(int postId,int userId) 
        {
            Post post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return new ErrorResponse { Success = false, Message = "Post not found" };
            }
            if (post.AuthorId != userId)
                return new ErrorResponse { Success = false, Message = "You can only delete your own posts" };

            foreach(var comment in post.Comments)
            {
                _commentRepository.Delete(comment);
            }

            foreach(var reaction in post.Reactions)
            {
                _reactionRepository.Remove(reaction);
            }

            _postRepository.Delete(post);
            if (_postRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Post deleted successfully" };

            return new ErrorResponse { Success = false, Message = "Could not delete post" };
        }
        public ErrorResponse PublishPost(int postId) 
        {
            Post post = _postRepository.GetPostById(postId);

            post.IsPublic = true;
            if (_postRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Post published successfully" };

            return new ErrorResponse { Success = false, Message = "Could not publish post" };

        }

        public List<PostResponse> GetFeed(int page, int? userId = null) 
        {
            int pageSize = 10;
            var feed = _postRepository.GetAllUnremoved()
                .Where(p => p.IsPublic == true ||
                (userId != null && p.CommunityId != null && p.Community != null && p.Community.Members.Any(m => m.UserId == userId)) ||
                  (userId != null && p.AuthorId == userId) ||
                    (userId != null &&
                   p.Author.Followers.Any(f => f.FollowerUserId == userId)))
                .OrderByDescending(p => p.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return mapper.Map<List<PostResponse>>(feed);

        }
   
         public ErrorResponse SavePost(int userId, int postId) 
        {
            var post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return new ErrorResponse { Success = false, Message = "Post not found" };
            }
            if(post.AuthorId == userId)
            {
                return new ErrorResponse { Success = false, Message = "You cannot save your own post" };
            }
            bool alreadySaved = _postRepository.IsPostSavedByUser(userId, postId);
            if (alreadySaved)
            {
                return new ErrorResponse { Success = false, Message = "Post already saved" };
            }

            PostSaved saved = new PostSaved
            {
                UserId = userId,
                PostId = postId
            };
            _postRepository.SavePost(saved);
            if (_postRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Post saved successfully" };

            return new ErrorResponse { Success = false, Message = "Could not save post" };
        }
        public List<PostResponse> GetSavedPosts(int userId)
        {
            var posts = _postRepository.GetSavedPosts(userId);
            return mapper.Map<List<PostResponse>>(posts);
        }

        public ErrorResponse UnsavePost(int userId, int postId)
        {
            bool isSaved = _context.postsSaved
                .Any(ps => ps.UserId == userId && ps.PostId == postId);

            if (!isSaved)
                return new ErrorResponse { Success = false, Message = "Post not saved!" };

            _postRepository.UnsavePost(userId, postId);

            if (_postRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Post unsaved successfully" };

            return new ErrorResponse { Success = false, Message = "Could not unsave post" };
        }
        public List<PostTagDto> GetPostTags(int postId)
        {
            var tag = _postRepository.GetPostTags(postId);
            return mapper.Map<List<PostTagDto>>(tag);
        }
        public void AddTag(int postId,int tagId)
        {
            Post post = _postRepository.GetPostById(postId);
            PostTag postTag = new PostTag();
            postTag.PostId = postId;
            postTag.TagId = tagId;
            _postRepository.AddTag(postTag);
            _postRepository.SaveChanges();
        }
        public void RemoveTag(int postId, int tagId)
        {
            Post post = _postRepository.GetPostById(postId);
            PostTag postTag = _postRepository.GetPostTagbyId(postId, tagId);

            _postRepository.RemoveTag(postTag);
            _postRepository.SaveChanges();
        }
        public PostResponse GetPost(int postId, int? userId = null)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null || post.IsRemoved)
            {
                return null;
            }
            if(post.CommunityId != null && !post.IsPublic)
            {
                if(userId == null)
                    return null;

                bool isMember = _context.communityMembers.Any(m => m.CommunityId == post.CommunityId && m.UserId == userId);
                if (!isMember)
                {
                    return null;
                }   
            }
            var response = mapper.Map<PostResponse>(post);

            if (userId != null)
            {
                var reaction = post.Reactions
                    .FirstOrDefault(r => r.UserId == userId);

                response.UserReaction = reaction?.Type.ToString();
            }

            return response;
        }
        public List<PostResponse> SearchPosts(int userId,string? keyword, string? language, string? tag)
        {
            var posts = _postRepository.SearchPosts(userId,keyword, language, tag);
            return mapper.Map<List<PostResponse>>(posts);
        }
        public int GetPostAuthorId(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            return post.AuthorId;
        }

        public List<PostResponse> GetUserPosts(int userId)
        {
            var posts = _postRepository.GetAllUnremoved()
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.DateCreated)
                .ToList();

            return mapper.Map<List<PostResponse>>(posts);
        }
    }
}
