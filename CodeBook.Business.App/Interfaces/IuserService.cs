using System;
using CodeBook.Models.App;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Middleware;
namespace CodeBook.Business.App.Interfaces
{
    public interface IuserService
    {
        ErrorResponse DeleteAccount(int userId);
        UserProfileResponse GetProfile(int userId);
        ErrorResponse UpdateProfile(int userId, UpdateProfileDto data);
        ErrorResponse Follow(int followerId, int followeeId);
        ErrorResponse Unfollow(int followerId, int followeeId);
        List<UserProfileResponse> SearchUsers(string keyword);
        List<UserProfileResponse> GetFollowers(int userId);
        List<UserProfileResponse> GetFollowing(int userId);
        UserProfileResponse FindByUsername(string username);
    }
}
