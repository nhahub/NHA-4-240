using System;
using CodeBook.Business.App.DTOs;
using CodeBook.Models.App;
namespace CodeBook.Business.App.Interfaces
{
    public interface ICommunityService
    {
        void CreateCommunity(CreateCommunityDto dto,int userId);
        void JoinCommunity(int communityId, CommunityMember newMember);
        void AssignRole(int CommunityId,int userId, AssignRoleDto dto);
        void UpdateCommunity(int CommunityId,UpdateCommunityDto dto);
        void DeleteCommunity(int CommunityId);
        CommunityDto GetCommunity(int CommunityId);
        void UnjoinCommunity(int communityId, int userId);
        List<CommunityDto> GetCommunities(int userId);
        public List<PostResponse> GetCommunityFeed(int communityId);
        List<CommunityDto> SearchCommunities(string keyword);
        List<CommunityDto> GetUnjoinedCommunities(int userId);
        List<CommunityDto> GetOwnedCommunities(int userId);

    }
}
