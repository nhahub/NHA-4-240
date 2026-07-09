using CodeBook.Business.App.Interfaces;
using CodeBook.Models.App;
using CodeBook.Data.App.IRepositories;
using System;
using CodeBook.Business.App.DTOs;
using Microsoft.AspNetCore.Routing.Constraints;


namespace CodeBook.Business.App.Services
{
    public class ModerationService : IModerationService
    {
        private readonly IPostRepository _postRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ICommentRepository _commentRepository;

        public ModerationService(IPostRepository postRepository, IReportRepository reportRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _reportRepository = reportRepository;
            _commentRepository = commentRepository;
        }

        public void RemovePost(int postId, int reportId, int removerId)
        {
            var post = _postRepository.GetPostById(postId);

            if (post != null)
            {
                post.IsRemoved = true;
                post.DateUpdated = DateTime.Now;
                var report = _reportRepository.GetReportbyId(reportId);
                if (report != null)
                {

                    var removal = new PostRemoval
                    {
                        PostId = postId,
                        RemoverId = removerId,
                        ReportId = reportId,
                        Reason = report.Reason,
                        DateCreated = DateTime.Now
                    };
                    _postRepository.AddRemovalRecord(removal);

                    report.Status = ReportStatus.Accepted;
                    report.DateUpdated = DateTime.UtcNow;
                    _reportRepository.Update(report);
                    _postRepository.SaveChanges();
                }
            }
        }
        public void RemoveComment(int commentId, int reportId, int removerId)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment != null)
            {
                comment.isRemoved = true;
                comment.DateUpdated = DateTime.Now;
                var report = _reportRepository.GetReportbyId(reportId);
                if (report != null)
                {
                    var removal = new CommentRemoval
                {
                    CommentId = commentId,
                    RemoverId = removerId,
                    ReportId = reportId,
                    Reason = report.Reason,
                    DateCreated = DateTime.Now
                };
                _commentRepository.AddRemovalRecord(removal);

                 report.Status = ReportStatus.Accepted;
                 report.DateUpdated = DateTime.UtcNow;
                 _reportRepository.Update(report);
                

                }
                _commentRepository.SaveChanges();
            }
        }
    }
}
