using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using AutoMapper;
using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using System;
using CodeBook.Business.App.Middleware;
namespace CodeBook.Business.App.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            this._reportRepository = reportRepository;
            this.mapper = mapper;
        }
        public ErrorResponse SubmitReport(int reporterId, ReportRequest request)
        {
            var report = new Report
            {
                ReporterId = reporterId,
                PostId = request.PostId,
                CommentId = request.CommentId,
                Reason = request.Reason,
                Description = request.Description,
                Status = ReportStatus.Pending,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            _reportRepository.Add(report);
           if( _reportRepository.SaveChanges()) return new ErrorResponse { Success = true, Message = "Report Submitted Successfully!" };
           else return new ErrorResponse { Success = false, Message = "Couldn't Submit Report!" };
        }

        public ErrorResponse UpdateReport (int repoterId, ReportRequest request)
        {
            Report report = null;

            if(request.CommentId != null)
            {
                report = _reportRepository.GetCommentReportbyReporter(repoterId,request.CommentId);
                if (report == null) return new ErrorResponse { Success = false, Message = "You didn't report this comment!" };
            }
            else if (request.PostId != null)
            {
                report = _reportRepository.GetPostReportbyReporter(repoterId, request.PostId);
                if (report == null) return new ErrorResponse { Success = false, Message = "You didn't report this Post!" };
            }
            //if not PostId nor CommentId present so no report exists.
            if (report == null) return new ErrorResponse { Success = false, Message = "No Report Found" };

            //if there is a report so
            report.DateUpdated = DateTime.UtcNow;
            report.Status = ReportStatus.Pending;
            report.Reason = request.Reason;
            report.Description = request.Description;
            _reportRepository.Update(report);
            if (_reportRepository.SaveChanges())
            {
                return new ErrorResponse { Success = true, Message = "Report Updated" };
            }
            return new ErrorResponse { Success = false, Message = "Couldn't Update" };
        }


        public List<ReportDTO> GetPendingReports()
        {
            var report = _reportRepository.GetPendingReports();
            return mapper.Map<List<ReportDTO>>(report);
        }

        public void UpdateReportStatus(int reportId, UpdateReportStatusDto dto)
        {
            var report = _reportRepository.GetReportbyId(reportId);
            if (report != null)
            {
                report.Status = Enum.Parse<ReportStatus>(dto.Status);
                report.DateUpdated = DateTime.UtcNow;
                _reportRepository.Update(report);
                _reportRepository.SaveChanges();
            }
        }
    }
}
