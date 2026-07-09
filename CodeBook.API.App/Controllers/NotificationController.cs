using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Methods;
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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly CurrentUserInfo _currentUserInfo;
        public NotificationController(INotificationService notificationService, CurrentUserInfo currentUserInfo)
        {
            _notificationService = notificationService;
            _currentUserInfo = currentUserInfo;
        }

        [HttpGet("getnotification")]
        [Authorize]
        public ActionResult GetNotification()
        {
            try
            {
                var notification = _notificationService.GetUserNotification(_currentUserInfo.GetCurrentUserId());
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("readNotification")]
        [Authorize]
        public ActionResult MarkAsRead(int id)
        {
            try
            {
                _notificationService.MarkAsRead(id);
                return Ok(new { message = "Marked As Read!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetUnreadCount")]
        [Authorize]
        public ActionResult GetUnreadCount()
        {
            try
            {
                var count = _notificationService.GetUnreadNotificationCount(_currentUserInfo.GetCurrentUserId());
                return Ok(new { unreadCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("readAllNotifications")]
        [Authorize]
        public ActionResult MarkAllAsRead()
        {
            try
            {
                _notificationService.MarkAllAsRead(_currentUserInfo.GetCurrentUserId());
                return Ok(new { message = "Marked All As Read!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
