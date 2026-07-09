using System;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Middleware;

namespace CodeBook.Business.App.Interfaces
{
	public interface IReportService
    {
        ErrorResponse SubmitReport(int reporterId, ReportRequest request);
        List<ReportDTO> GetPendingReports();
        ErrorResponse UpdateReport(int repoterId, ReportRequest request);
        void UpdateReportStatus(int reportId, UpdateReportStatusDto dto);
    }
}
