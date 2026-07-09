
using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface IPostRepository
    {
        Post GetPostById(int postid);
        void Add(Post post);
        void Update(Post post);
        void Delete(Post post);
        IQueryable<Post> GetAllUnremoved();
        void AddReaction(int postId, ReactionType type,int userId);
        void RemoveReaction(int postId);
        void AddComment(int postId);
        void RemoveComment(int postId,int count);
        List<Post> Getfeed();
        void SavePost(PostSaved saved);
        void AddRemovalRecord(PostRemoval postRemoval);
        List<PostTag> GetPostTags(int postId);
        void AddTag(PostTag tag);
        void RemoveTag(PostTag tag);
        PostTag GetPostTagbyId(int postId, int tagId);
        bool IsPostSavedByUser(int userId, int postId);
        List<Post> GetSavedPosts(int userId);
        void UnsavePost(int userId, int postId);
        PostSaved GetSavedPost(int userId, int postId);
        List<Post> SearchPosts(int userId, string? keyword, string? language, string? tag);
        bool SaveChanges();
       
    }

}
