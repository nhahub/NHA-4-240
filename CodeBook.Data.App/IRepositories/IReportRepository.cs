using CodeBook.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Data.App.IRepositories
{
    public interface IReportRepository
    {
        List<Report> GetPendingReports();
        void Add(Report report);
        void Update(Report report);
        void Delete(Report report);
        Report GetReportbyId(int? reportId);
        Report GetCommentReportbyReporter(int reporterId, int? CommentId);
        Report GetPostReportbyReporter(int reporterId, int? postId);
        bool SaveChanges();

    }
}
