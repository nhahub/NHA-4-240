using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using CodeBook.Business.App.Validators;


namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy ="AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IModerationService _moderationService;
        private readonly IReportService _reportService;
        private readonly CurrentUserInfo _currentUserInfo;


        public AdminController(IModerationService moderationService, IReportService reportService, CurrentUserInfo currentUserInfo)
        {
            _moderationService = moderationService;
            _reportService = reportService;
            _currentUserInfo = currentUserInfo;
        }

     

        [HttpDelete("posts/{postId}/{reportId}")]
        public IActionResult RemovePost(int postId, int reportId)
        {
            try
            {
                var removerId = _currentUserInfo.GetCurrentUserId();
                if (removerId == 0) throw new KeyNotFoundException();
                _moderationService.RemovePost(postId, reportId ,removerId);
                return Ok(new { message = "Post removed successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex) {               
                return BadRequest(ex.Message);
             }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error post removal: {ex.Message}, {ex.InnerException?.Message}");
            }
        }
        [HttpDelete("comments/{commentId}/{reportId}")]
        public IActionResult RemoveComment(int commentId,int reportId)
        {
            try
            {
                var removerId = _currentUserInfo.GetCurrentUserId();
                if(removerId == 0) throw new KeyNotFoundException();
                _moderationService.RemoveComment(commentId, reportId, removerId);
                return Ok(new { message = "Comment removed successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error comment removal: {ex.Message}");
            }
        }
        [HttpGet("reports")]
        public IActionResult GetReports()
        {
            try
            {
                var reports = _reportService.GetPendingReports();
               /* if (!reports.Any())
                    return NotFound("No pending reports found.");*/
                return Ok(reports);
            }
            catch (Exception ex) { 
            return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("reports/{id}/status")]
        public IActionResult UpdateReportStatus(int id, [FromBody] UpdateReportStatusDto dto)
        {
            try { 
             _reportService.UpdateReportStatus(id, dto);
                return Ok(new { message = "Report Status Updated Successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
    }
}
