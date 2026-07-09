using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Methods;
using CodeBook.Business.App.Middleware;
using CodeBook.Business.App.Services;
using CodeBook.Business.App.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly AbstractValidator<ReportRequest> _reportValidator;
        private readonly CurrentUserInfo _currentUserInfo;
        public ReportController(IReportService reportService, AbstractValidator<ReportRequest> reportValidator, CurrentUserInfo currentUserInfo)
        {
            _reportService = reportService;
            _reportValidator = reportValidator;
            _currentUserInfo = currentUserInfo;
        }

        [HttpPost("submitreport")]
        [Authorize]
        public IActionResult SubmitReport([FromBody] ReportRequest request)
        {
            var validationResult = _reportValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            if (request.CommentId == 0) request.CommentId = null;
            if (request.PostId == 0) request.PostId = null;
            try
            {
                var result = _reportService.SubmitReport(_currentUserInfo.GetCurrentUserId(), request);
                if (result != null && result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPatch("updatereport")]
        [Authorize]
        public IActionResult UpdateReport([FromBody] ReportRequest request)
        {
            var validationResult = _reportValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                ErrorResponse result = _reportService.UpdateReport(_currentUserInfo.GetCurrentUserId(), request);
                if (result != null && result.Success)
                {
                    return Ok(new { message = result.Message });
                }
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
