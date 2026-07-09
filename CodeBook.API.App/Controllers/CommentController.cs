using CodeBook.Business.App.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeBook.Business.App.Services;
using CodeBook.Business.App.Middleware;
using CodeBook.Business.App.Interfaces;

namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly CurrentUserInfo _currentUserInfo;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, CurrentUserInfo currentUserInfo, IPostService postService)
        {
            _commentService = commentService;
            _currentUserInfo = currentUserInfo;
            _postService = postService;
        }

        [HttpGet("{postId}/comments")]
        [AllowAnonymous]
        public IActionResult GetComments(int postId)
        {
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = _currentUserInfo.GetCurrentUserId();
            }
            try
            {
                var post = _postService.GetPost(postId, userId);
                if (post == null)
                {
                    return NotFound(new { message = "Post not found or access denied" });
                }
                var comments = _commentService.GetPostComments(postId);
                if (userId != null && comments.Any())
                {
                    foreach (var comment in comments)
                    {
                        if (comment.AuthorId == userId) comment.isOwner = true;
                    }

                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{postId}/comments")]
        [Authorize]
        public IActionResult AddComment(int postId, [FromBody] AddCommentRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var result = _commentService.AddComment(userId, postId, request);
                if (result.Success)
                    return Ok(new { message = result.Message });
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{commentId}/deleteComment")]
        [Authorize]
        public IActionResult DeleteComment(int commentId)
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = _commentService.DeleteComment(commentId, userId);
                if (result.Success)
                    return Ok(new { message = result.Message });
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{commentId}/editComment")]
        [Authorize]
        public IActionResult EditComment(int commentId, [FromBody] EditCommentRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request" });

            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = _commentService.EditComment(commentId, request.Body, userId);
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
