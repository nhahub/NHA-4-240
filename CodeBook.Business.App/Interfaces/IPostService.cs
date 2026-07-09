using CodeBook.Business.App.DTOs;
using CodeBook.Models.App;
using CodeBook.Business.App.Middleware;
using System;
namespace CodeBook.Business.App.Interfaces
{
    public interface IPostService
    {
        ErrorResponse CreatePost(int userId,CreatePostRequest request, int? communityId);
        ErrorResponse UpdatePost(int postId, UpdatePostRequest request,int userId);
        ErrorResponse DeletePost(int postId, int userId);
        ErrorResponse PublishPost(int postId);
        List<PostResponse> GetFeed(int page,int? userId = null);
        ErrorResponse SavePost(int userid, int postId);
        List<PostTagDto> GetPostTags(int postId);
        void AddTag(int postId, int tagId);
        void RemoveTag(int postId, int tagId);
        PostResponse GetPost(int postId, int? userId = null);
        int GetPostAuthorId(int postId);

        List<PostResponse> SearchPosts(int userId, string? keyword, string? language, string? tag);
        List<PostResponse> GetSavedPosts(int userId);
        ErrorResponse UnsavePost(int userId, int postId);
        List<PostResponse> GetUserPosts(int userId);
    }
}
