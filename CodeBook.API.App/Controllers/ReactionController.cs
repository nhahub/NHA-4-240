using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Methods;
using CodeBook.Business.App.Middleware;
using CodeBook.Business.App.Services;
using CodeBook.Business.App.Validator;
using CodeBook.Models.App;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly CurrentUserInfo _currentUserInfo;

        public ReactionController(IReactionService reactionService, CurrentUserInfo currentUserInfo)
        {
            _reactionService = reactionService;
            _currentUserInfo = currentUserInfo;
        }


        [HttpPost("addPostreaction")]
        [Authorize]
        public ActionResult AddPostReaction([FromBody] ReactionDto reactionDto)
        {
            var currentId = _currentUserInfo.GetCurrentUserId();
            try
            {
                ErrorResponse result = _reactionService.AddPostReaction(currentId, reactionDto);
                if (result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("addCommentreaction")]
        [Authorize]
        public ActionResult AddCommentReaction([FromBody] ReactionDto reactionDto)
        {
            var currentId = _currentUserInfo.GetCurrentUserId();
            if (reactionDto.CommentId == null)
                return BadRequest(new { message = "CommentId is required" });
            try
            {
                ErrorResponse result = _reactionService.AddCommentReaction(currentId, reactionDto);
                if (result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("removePostreaction")]
        [Authorize]
        public ActionResult RemovePostReaction(int postId)
        {
            var currentId = _currentUserInfo.GetCurrentUserId();
            try
            {
                ErrorResponse result = _reactionService.RemovePostReaction(postId, currentId);
                if (result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("removeCommentreaction")]
        [Authorize]
        public ActionResult RemoveCommentReaction(int postId,int commentId)
        {
            var currentId = _currentUserInfo.GetCurrentUserId();
            try
            {
                ErrorResponse result = _reactionService.RemoveCommentReaction(currentId, commentId);
                if (result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
