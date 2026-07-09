using AutoMapper;
using Azure;
using BCrypt.Net;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Mapping;
using CodeBook.Business.App.Middleware;
using CodeBook.Data.App.IRepositories;
using CodeBook.Data.App.Repositories;
using CodeBook.Models.App;
using System;

namespace CodeBook.Business.App.Services
{
    public class UserService : IuserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowRepository _followRepository;
        private readonly INotificationService _notificationService;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper mapper;
        public UserService(IUserRepository userRepository, IFollowRepository followRepository, IMapper mapper, INotificationService notificationService, IPostRepository postRepository, ICommentRepository commentRepository, IReactionRepository reactionRepository, IReportRepository reportRepository)
        {
            _userRepository = userRepository;
            _followRepository = followRepository;
            this.mapper = mapper;
            _notificationService = notificationService;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _reactionRepository = reactionRepository;
            _reportRepository = reportRepository;
        }
        public ErrorResponse DeleteAccount(int userId) 
        {
            User user = _userRepository.GetProfileById(userId);
            if(user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" };
            deleteUserRelatedRecords(user);
            _userRepository.Remove(user);
            if (_userRepository.SaveChanges())
            {
                var reports = _reportRepository.GetPendingReports().Where(report => report.PostId == null && report.CommentId == null).ToList();
                foreach (var report in reports)
                {
                    _reportRepository.Delete(report);
                }
                _reportRepository.SaveChanges();
                return new ErrorResponse { Success = true, Message = "Profile Deleted!" };
            }
            return new ErrorResponse { Success = false, Message = "Couldn't Delete!" };

        }

        public void deleteUserRelatedRecords(User user)
        {
            foreach (var report in user.Reports)
            {
                _reportRepository.Delete(report);
            }

            foreach (var post in user.Posts)
            {
                _postRepository.Delete(post);
            }

            foreach (var comment in user.Comments)
            {
                _commentRepository.Delete(comment);
            }

            foreach (var reaction in user.Reactions)
            {
                _reactionRepository.Remove(reaction);
            }
            foreach (var followee in user.Followers)
            {
                _followRepository.RemoveFollow(followee);
            }
            foreach(var post in user.SavedPosts)
            {
                _postRepository.UnsavePost(user.Id, post.PostId);
            }

        }
        public UserProfileResponse GetProfile(int userId)
        {
            User user = _userRepository.GetProfileById(userId);
            if (user == null) return null;
           var response = mapper.Map<UserProfileResponse>(user);     
            response.FollowersCount = _followRepository.GetFollowersCount(userId);
            response.FollowingCount = _followRepository.GetFollowingCount(userId);
            return response;
        }
        public ErrorResponse UpdateProfile(int userId,UpdateProfileDto data) 
        {
            User user = _userRepository.GetProfileById(userId);
            if(user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" } ;

            user.Bio = data.Bio;
            user.UserName = data.UserName;
            user.AvatarUrl = data.AvatarUrl;
            _userRepository.Update(user);
            if(_userRepository.SaveChanges()) return new ErrorResponse { Success = true, Message = "Profile Updated!" };
            return new ErrorResponse { Success = false, Message = "Couldn't Update!" };

        }
        public ErrorResponse Follow(int followerId, int followeeId)
        {
            User user = _userRepository.GetProfileById(followeeId);
            if (user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" };

            Follow followedalready = _followRepository.GetFollow(followerId, followeeId);
            if (followedalready != null) return new ErrorResponse { Success = false, Message = "Already Followed!" };

            Follow follow = new Follow();
            follow.FollowerUserId = followerId;
            follow.FolloweeUserId = followeeId;
            _followRepository.AddFollow(follow);
            User follower = _userRepository.GetProfileById(followerId);
            bool result =  _followRepository.SaveChanges();
            if (result)
            {
                 _notificationService.CreateNotification(followeeId, new NotificationDTO
                {
                    userId = followeeId,
                    Type = "Follow",
                    Message = follower.UserName + " followed you",
                    ReferenceId = followerId,
                    IsSeen = false,
                    DateCreated = DateTime.UtcNow,
                    SenderId = followerId
                 });
                return new ErrorResponse { Success = true, Message = "Followed!" };

            }
            return new ErrorResponse { Success = false, Message = "Couldn't Follow!" };

        }

        public ErrorResponse Unfollow(int followerId, int followeeId)
        {
            User user = _userRepository.GetProfileById(followeeId);
            if (user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" };

            Follow follow = _followRepository.GetFollow(followerId, followeeId);
            if (follow == null) return new ErrorResponse { Success = false, Message = "You don't Follow him already!" };

            _followRepository.RemoveFollow(follow);
            bool result = _followRepository.SaveChanges();

            if (result) { return new ErrorResponse { Success = true, Message = "Unfollowed!" }; }
            return new ErrorResponse { Success = false, Message = "Couldn't Unfollow!" };

        }
        public List<UserProfileResponse> SearchUsers(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return new List<UserProfileResponse>();
            }

            var users = _userRepository.SearchUsers(keyword);
            return mapper.Map<List<UserProfileResponse>>(users);
        }
        public List<UserProfileResponse> GetFollowers(int userId)
        {
            var followers = _followRepository.GetFollowers(userId);
            return mapper.Map<List<UserProfileResponse>>(followers);
        }
        public List<UserProfileResponse> GetFollowing(int userId)
        {
            var following = _followRepository.GetFollowing(userId);
            return mapper.Map<List<UserProfileResponse>>(following);
        }

        public UserProfileResponse FindByUsername(string username)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null) return null;
            return mapper.Map<UserProfileResponse>(user);
        }
    }
}
