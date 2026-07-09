using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly CodeBookContext _context;
        public CommunityRepository(CodeBookContext context)
        {
            _context = context;
        }

        public Community GetCommunity(int communityId)
        {
            Community community = _context.communities.Include(c => c.Members).FirstOrDefault(c => c.Id == communityId);
            if (community == null)
                throw new Exception("Community Not Found!!");
            return community;
        }

        public List<Community> GetCommunities(int userId)
        {
            return _context.communities.Where(c => c.Members.Any(m => m.UserId == userId)).Include(c => c.Members).ToList();
        } 

        public List<Community> GetUnjoinedCommunities(int userId)
        {
            var joined = _context.communityMembers.Where(cm => cm.UserId == userId).Select(cm => cm.CommunityId);
            return _context.communities.Include(c => c.Members).Where(c => !joined.Contains(c.Id)).ToList();
        }

        public void Add(Community community)
        {
            _context.communities.Add(community);
        }

        public void Update(Community community)
        {
            _context.communities.Update(community);
        }

        public void Delete(Community community)
        {
            _context.communities.Remove(community);
        }

        public void JoinCommunity(CommunityMember member)
        {
            _context.communityMembers.Add(member);
        }
        public CommunityMember GetCommunityMember(int communityid, int userid)
        {
            CommunityMember member = _context.communityMembers.FirstOrDefault(m => m.CommunityId == communityid && m.UserId == userid);

            if (member == null)
                throw new KeyNotFoundException("Member Not Found!!");
            return member;
        }

        public void UpdateCommunityMember(CommunityMember member)
        {
            _context.communityMembers.Update(member);
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
        public void RemoveMember(CommunityMember member)
        {
            _context.communityMembers.Remove(member);
        }

        public List<Community> SearchCommunities(string keyword)
        {
            return _context.communities
                .Where(c => c.Name.Contains(keyword) || c.Description.Contains(keyword))
                .ToList();
        }

        public List<Community> GetOwnedCommunities(int userId)
        {
            return _context.communities.Include(c => c.Members)
                .Where(c => c.OwnerId == userId).ToList();
        }
        public Community GetCommunitybyOwnerandDate(int userId, DateTime DateCreated)
        {
            return _context.communities.Include(c => c.Members)
                .FirstOrDefault(c => c.OwnerId == userId && c.DateCreated == DateCreated);
        }
    }
}
