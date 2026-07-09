using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Middleware;
using System;
namespace CodeBook.Business.App.Interfaces
{
    public interface ICommentService
    {
        ErrorResponse AddComment(int authorId, int postId, AddCommentRequest request);
        ErrorResponse DeleteComment(int commentId, int userId);
        ErrorResponse EditComment(int commentId, string commentBody, int userId);
        List<CommentDto> GetPostComments(int postId);
        int GetCommentAuthorId(int commentId);
    }
}
