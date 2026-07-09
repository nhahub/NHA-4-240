using CodeBook.Business.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeBook.Business.App.DTOs;
using System.Security.Claims;
using CodeBook.Models.App;
using CodeBook.Business.App.Middleware;

namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {

        private readonly IuserService _userService;
        private readonly CurrentUserInfo _currentUserInfo;
        public UserController(IuserService userService, CurrentUserInfo currentUserInfo)
        {
            _userService = userService;
            _currentUserInfo = currentUserInfo;
        }


        [HttpGet("viewprofile")]
        [AllowAnonymous]
        public IActionResult GetProfile(int userId)
        {
            try
            {
                UserProfileResponse userProfile = _userService.GetProfile(userId);
                if (userProfile == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("viewmyprofile")]
        [Authorize]
        public IActionResult GetMyProfile()
        {
            int userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                UserProfileResponse userProfile = _userService.GetProfile(userId);
                if (userProfile == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("deletemyprofile")]
        [Authorize]
        public IActionResult DeleteProfile()
        {
            var currentid = _currentUserInfo.GetCurrentUserId();
            try
            {
                ErrorResponse result = _userService.DeleteAccount(currentid);
                if (result.Success)
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

        [HttpPatch("updatemyprofile")]
        [Authorize]
        public IActionResult UpdateProfile(UpdateProfileDto updateProfile)
        {
            var currentid = _currentUserInfo.GetCurrentUserId();
            try
            {
                ErrorResponse result = _userService.UpdateProfile(currentid, updateProfile);
                if (result.Success)
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

        [HttpPost("follow")]
        [Authorize]
        public IActionResult Follow(int userid)
        {
            var currentid = _currentUserInfo.GetCurrentUserId();
            if (currentid == userid)
            {
                return BadRequest(new { message = "You cannot follow yourself!" });
            }
            try
            {
                ErrorResponse result = _userService.Follow(currentid, userid);


                if (result.Success)
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

        [HttpDelete("unfollow")]
        [Authorize]
        public IActionResult Unfollow(int userid)
        {
            var currentid = _currentUserInfo.GetCurrentUserId();
            if (currentid == userid)
            {
                return BadRequest(new { message = "You cannot unfollow yourself!" });
            }
            try
            {
                ErrorResponse result = _userService.Unfollow(currentid, userid);
                if (result.Success)
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
        [HttpGet("followers")]
        [Authorize]
        public IActionResult GetFollowers()
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var followers = _userService.GetFollowers(userId);
                return Ok(followers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("followings")]
        [Authorize]
        public IActionResult GetFollowing()
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var following = _userService.GetFollowing(userId);
                return Ok(following);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchUsers([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest(new { message = "keyword is required" });
            }
            try
            {
                var users = _userService.SearchUsers(keyword).Where(u => u.Role != UserRole.Admin);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("findByUsername")]
        [AllowAnonymous]
        public IActionResult FindByUsername([FromQuery] string username)
        {
            try
            {
                var user = _userService.FindByUsername(username);
                if (user == null)
                    return NotFound(new { message = "User not found" });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
