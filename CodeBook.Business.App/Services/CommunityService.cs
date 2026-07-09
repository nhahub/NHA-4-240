
using AutoMapper;
﻿using AutoMapper.Execution;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Data.App.IRepositories;
using CodeBook.Data.App.Repositories;
using CodeBook.Models.App;
﻿using System;
using System;

namespace CodeBook.Business.App.Services

{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly INotificationService _notificationService;
        private readonly IPostRepository _postRepository;
        private readonly IMapper mapper;

        public CommunityService(ICommunityRepository communityRepository,INotificationService notificationService,IMapper mapper,IPostRepository postRepository)
        {
            _communityRepository = communityRepository;
            _notificationService = notificationService;
            _postRepository = postRepository;
            this.mapper = mapper;
        }
        public void CreateCommunity(CreateCommunityDto dto,int userId)
        {
            Community community = new Community();
            community.OwnerId = userId;
            community.Name = dto.Name;
            community.Description = dto.Description;
            community.IconURL = dto.IconURL;
            _communityRepository.Add(community);
            _communityRepository.SaveChanges();
            community = _communityRepository.GetCommunitybyOwnerandDate(userId,community.DateCreated);
            var member = new CommunityMember
            {
                CommunityId = community.Id,
                UserId = userId,
                Role = CommunityRole.Admin,
                JoinedAt = DateTime.UtcNow
            };
            _communityRepository.JoinCommunity(member);
            _communityRepository.SaveChanges();

        }
        public void UpdateCommunity(int CommunityId,UpdateCommunityDto dto)
        {
            Community community = _communityRepository.GetCommunity(CommunityId);
            community.Name = dto.Name;
            community.Description = dto.Description;
            community.IconURL = dto.IconURL;
            _communityRepository.Update(community);
            _communityRepository.SaveChanges();

        }
        public void DeleteCommunity(int CommunityId)
        {
            Community community = _communityRepository.GetCommunity(CommunityId);

            _communityRepository.Delete(community);
            _communityRepository.SaveChanges();
        }
        public void JoinCommunity(int communityId,CommunityMember newMember)
        {
            //why get community?
            Community community = _communityRepository.GetCommunity(communityId);

            _communityRepository.JoinCommunity(newMember);
            _communityRepository.SaveChanges();

            if (community.OwnerId != null)
            {
                _notificationService.CreateNotification((int)community.OwnerId, new NotificationDTO
                {
                    userId = (int)community.OwnerId,
                    Type = "Join",
                    Message = "You have a new Community Member",
                    ReferenceId = communityId,
                    IsSeen = false,
                    DateCreated = DateTime.UtcNow
                });
            }

        }
        public void AssignRole(int CommunityId,int userId,AssignRoleDto dto)
        {
            CommunityMember member = _communityRepository.GetCommunityMember(CommunityId, userId);

            member.Role = dto.Role;
            _communityRepository.UpdateCommunityMember(member);
            _communityRepository.SaveChanges();

        }
        public CommunityDto GetCommunity(int CommunityId)
        {
            Community community = _communityRepository.GetCommunity(CommunityId);
            return mapper.Map<CommunityDto>(community) ;
        }
        public void UnjoinCommunity(int communityId,int userId)
        {
            CommunityMember member = _communityRepository.GetCommunityMember(communityId, userId);
            if(member == null)
                throw new KeyNotFoundException("Member Not Found!!");
            Community community = _communityRepository.GetCommunity(communityId);
            if (community.OwnerId == userId)
                throw new UnauthorizedAccessException("Owner cannot unjoin his community!");
            _communityRepository.RemoveMember(member);
            _communityRepository.SaveChanges();
        }
        public List<CommunityDto> GetCommunities(int userId)
        {
            List<Community> communities = _communityRepository.GetCommunities(userId);
            return mapper.Map<List<CommunityDto>>(communities);
        }

        public List<PostResponse> GetCommunityFeed(int communityId)
        {
            var feed = _postRepository.GetAllUnremoved()
                .Where(p => p.CommunityId == communityId && p.Community != null)
                .OrderByDescending(p => p.DateCreated)
                .ToList();
            return mapper.Map<List<PostResponse>>(feed);

        }

        public List<CommunityDto> SearchCommunities(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return new List<CommunityDto>();
            }
            var communities = _communityRepository.SearchCommunities(keyword);
            return mapper.Map<List<CommunityDto>>(communities);
        }

        public List<CommunityDto> GetUnjoinedCommunities(int userId)
        {
            var unjoined = _communityRepository.GetUnjoinedCommunities(userId);
            return mapper.Map<List<CommunityDto>>(unjoined);
        }
        public List<CommunityDto> GetOwnedCommunities(int userId)
        {
            var communities = _communityRepository.GetOwnedCommunities(userId);
            var communitiesresponse = mapper.Map<List<CommunityDto>>(communities);
            foreach (var community in communitiesresponse)
            {
                community.isOwner = true;
            }
            return communitiesresponse;
        }

    }
}
