
﻿using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Middleware;
using CodeBook.Data.App.IRepositories;
using CodeBook.Data.App.IRepositories;
using CodeBook.Data.App.Repositories;
using CodeBook.Models.App;
using CodeBook.Models.App;
using Microsoft.AspNetCore.SignalR;
using System;
﻿using System;

namespace CodeBook.Business.App.Services
{

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationService _notificationService;
        private readonly IPostRepository _postRepository;  
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository,INotificationService notificationService, IPostRepository postRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _notificationService = notificationService;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public ErrorResponse AddComment(int userId, int postId, AddCommentRequest dto)
        {
            var post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return new ErrorResponse { Success = false, Message = "Post not found"};
            }
            if (post.IsRemoved)
            {
                return new ErrorResponse { Success = false, Message = "Post is no longer available" };
            }


            Comment comment = new Comment
            {
                AuthorId = userId,
                PostId = postId,
                Body = dto.Body,
                SelfCommentId = dto.SelfCommentId == 0 ? null: dto.SelfCommentId,
                DateCreated = DateTime.UtcNow,
            };
            _commentRepository.Add(comment);
            _postRepository.AddComment(postId);
            if (!_commentRepository.SaveChanges())
                return new ErrorResponse { Success = false, Message = "Could not add comment" };

            var user = _userRepository.GetProfileById(userId);
            var postauthor = _postRepository.GetPostById(postId).AuthorId;
            _notificationService.CreateNotification(postauthor, new NotificationDTO
            {
                userId = postauthor,
                Type = "Comment",
                Message = $"{user.UserName} commented on your post",
                ReferenceId = postId,
                IsSeen = false,
                DateCreated = DateTime.UtcNow
            });
            return new ErrorResponse { Success = true, Message = "Comment added successfully" };

        }
        public ErrorResponse EditComment(int commentId,string commentBody, int userId)
        {
            var comment = _commentRepository.GetCommentById(commentId);
            if (comment == null)
            {
                return new ErrorResponse { Success = false, Message = "Comment not found" };
            }
            if(comment.AuthorId != userId)
            {
                return new ErrorResponse { Success = false, Message = "You can only edit your own comments" };
            }

            comment.Body = commentBody == "string" ? comment.Body : commentBody;
            
            _commentRepository.Update(comment);
            if (_commentRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Comment updated successfully" };

            return new ErrorResponse { Success = false, Message = "Could not update comment" };

        }
        public ErrorResponse DeleteComment(int commentId, int userId)
        {
            var comment = _commentRepository.GetCommentById(commentId);
            if (comment == null)
            {
                return new ErrorResponse { Success = false, Message = "Comment not found" };
            }
            if (comment.AuthorId != userId)
            {
                return new ErrorResponse { Success = false, Message = "You can only delete your own comments" };
            }
            int deletedComments=_commentRepository.Delete(comment);
            _postRepository.RemoveComment(comment.PostId,deletedComments);
            if (_commentRepository.SaveChanges())
                return new ErrorResponse { Success = true, Message = "Comment deleted successfully" };

            return new ErrorResponse { Success = false, Message = "Could not delete comment" };
        }

        public List<CommentDto> GetPostComments(int postId)
        {
            var comments = _commentRepository.GetByPostId(postId);

            if(comments == null || !comments.Any())
            {
                return new List<CommentDto>();
            }

            return comments
                .Where(c => !c.isRemoved)
                .Select(c => new CommentDto
            {
                Id = c.Id,
                AuthorId = c.AuthorId,
                AuthorUsername = c.Author.UserName,
                Body = c.Body,
                LikeCount = c.LikeCount,
                SelfCommentId = c.SelfCommentId,
                DateCreated = c.DateCreated
            }).ToList();
        }
        public int GetCommentAuthorId(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            return comment.AuthorId;
        }
    }
}
