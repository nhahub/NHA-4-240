using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface ICommunityRepository
    {
        Community GetCommunity(int communityid);
        void Add(Community community);
        void Update(Community community);
        void Delete(Community community);
        void JoinCommunity(CommunityMember member);
        CommunityMember GetCommunityMember(int communityid, int userid);
        void UpdateCommunityMember(CommunityMember member);
        bool SaveChanges();
        void RemoveMember(CommunityMember member);
        List<Community> GetCommunities(int userId);
        List<Community> SearchCommunities(string keyword);
        List<Community> GetUnjoinedCommunities(int userId);
        List<Community> GetOwnedCommunities(int userId);
        Community GetCommunitybyOwnerandDate(int userId, DateTime DateCreated);
    }
}
