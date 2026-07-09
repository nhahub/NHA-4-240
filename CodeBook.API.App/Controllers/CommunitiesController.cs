using CodeBook.API.App.Controllers;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace CodeBook.Business.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommunitiesController : ControllerBase
    {
        private readonly ICommunityService _communityService;
        private readonly CurrentUserInfo _currentUserInfo;

        public CommunitiesController(ICommunityService communityService, CurrentUserInfo currentUserInfo)
        {
            _communityService = communityService;
            _currentUserInfo = currentUserInfo;
        }

        [HttpPost("createcommunity")]
        public IActionResult CreateCommunity([FromBody] CreateCommunityDto dto) {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                if (string.IsNullOrEmpty(dto.Name))
                    return BadRequest("Community name cannot be empty.");

                _communityService.CreateCommunity(dto, userId);
                return Ok(new { message = "Community Created Successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPatch("{communityId}/updatecommunity")]
        public IActionResult UpdateCommunity(int communityId, [FromBody] UpdateCommunityDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Name))
                    return BadRequest("Community name cannot be empty.");
                _communityService.UpdateCommunity(communityId, dto);
                return Ok(new { message = "Community Updated Successfully" });
            }
            catch (KeyNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{communityId}/deletecommunity")]
        public IActionResult DeleteCommunity(int communityId)
        {
            try {
                _communityService.DeleteCommunity(communityId);
                return Ok(new { message = "Community Deleted Successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("{communityId}/joincommunity")]
        public IActionResult JoinCommunity(int communityId, [FromBody] JoinCommunityDto dto) {
            try {
                var member = new CommunityMember
                {
                    CommunityId = communityId,
                    UserId = _currentUserInfo.GetCurrentUserId(),
                    Role = Enum.Parse<CommunityRole>(dto.Role),
                    JoinedAt = DateTime.UtcNow
                };
                _communityService.JoinCommunity(communityId, member);
                return Ok(new { message = "Joined Community Successfully" });
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpPost("{communityId}/assignrole")]
        public IActionResult AssignRole(int communityId, [FromBody] AssignRoleDto dto)
        {
            try {
                var userId = _currentUserInfo.GetCurrentUserId();
                _communityService.AssignRole(communityId, userId, dto);
                return Ok(new { message = "Role Assigned Successfully" });
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchCommunities([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest(new { message = "keyword is required" });
            }
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var communities = _communityService.SearchCommunities(keyword);
                foreach (var community in communities)
                {
                    if (community.OwnerId == userId) community.isOwner = true;
                }
                return Ok(communities);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{communityId}/getcommunity")]
        public IActionResult GetCommunity(int communityId)
        {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                CommunityDto community = _communityService.GetCommunity(communityId);
                if(community.OwnerId == userId) community.isOwner = true;
                return Ok(community);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{communityId}/unjoin")]
        public IActionResult UnjoinCommunity(int communityId)
        {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                _communityService.UnjoinCommunity(communityId, userId);
                return Ok(new { message = "Unjoined Community Successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getCommunities")]
        public IActionResult GetCommunities()
        {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var communities = _communityService.GetCommunities(userId);
                foreach(var community in communities)
                {
                    if(community.OwnerId == userId) community.isOwner = true;
                }

                return Ok(communities);
            }
            catch (Exception e) {
                return BadRequest(new { message = "Couldn't load Communities: " + e.Message });
            }

        }

        [HttpGet("{communityId}/getCommunityFeed")]
        public IActionResult GetCommunityFeed(int communityId)
        {
            try
            {
                var feed = _communityService.GetCommunityFeed(communityId);
                return Ok(feed);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Couldn't get community feed: "+ e.Message });
            }
        }

        [HttpGet("getunjoinedcommunities")]
        public IActionResult GetUnjoinedCommunities()
        {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var communities = _communityService.GetUnjoinedCommunities(userId);
                return Ok(communities);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Couldn't get communities: " + e.Message });
            }
        }

        [HttpGet("getownedcommunities")]
        public IActionResult GetOwnedCommunities() {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var communities = _communityService.GetOwnedCommunities(userId);
                return Ok(communities);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Couldn't get communities: " + e.Message });
            }
        }

    }
}
