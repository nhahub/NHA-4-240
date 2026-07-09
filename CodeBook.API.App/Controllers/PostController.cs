using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.AccessControl;
using System.Security.Claims;


namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly CurrentUserInfo _currentUserInfo;

        public PostController(IPostService postService, ICommentService commentService, CurrentUserInfo currentUserInfo)
        {
            _postService = postService;
            _commentService = commentService;
            _currentUserInfo = currentUserInfo;
        }
        [HttpGet("feed")]
        [AllowAnonymous]
        public IActionResult GetFeed([FromQuery] int page = 1)
        {
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            { 
                userId = _currentUserInfo.GetCurrentUserId();
            }
            try
            {
                var feed = _postService.GetFeed(page, userId);

                return Ok(feed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public IActionResult GetPost(int postId)
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
                if (userId != null && post.AuthorId == userId) { post.isOwner = true; }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreatePost([FromBody] CreatePostRequest request)
        {
            if (request == null)
            {    
                return BadRequest(new { message = "Invalid request" });
            }
            request.TagIds ??= new List<int>();
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = request.CommunityId == 0
                    ? _postService.CreatePost(userId, request, null)
                    : _postService.CreatePost(userId, request, request.CommunityId);

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

        [HttpPut("{postId}/update")]
        [Authorize]
        public IActionResult UpdatePost(int postId, [FromBody] UpdatePostRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var result = _postService.UpdatePost(postId, request, userId);
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

        [HttpDelete("{postId}/deletePost")]
        [Authorize]
        public IActionResult DeletePost(int postId)
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = _postService.DeletePost(postId, userId);

                return Ok(new { message = result.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.InnerException.Message });

            }

        }

        [HttpPost("{postId}/save")]
        [Authorize]
        public IActionResult SavePost(int postId)
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = _postService.SavePost(userId, postId);
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

        [HttpGet("saved")]
        [Authorize]
        public IActionResult GetSavedPosts()
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var posts = _postService.GetSavedPosts(userId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/unsave")]
        [Authorize]
        public IActionResult UnsavePost(int id)
        {
            var userId = _currentUserInfo.GetCurrentUserId();
            try
            {
                var result = _postService.UnsavePost(userId, id);

                if (result.Success)
                    return Ok(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{postId}/tags")]
        [AllowAnonymous]
        public IActionResult GetPostTag(int postId)
        {
            try
            {
                var tags = _postService.GetPostTags(postId);
                if (tags == null || !tags.Any())
                {
                    return NotFound(new { message = "No tags found for this post" });
                }
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchPosts([FromQuery] string? keyword,
                                  [FromQuery] string? language,
                                  [FromQuery] string? tag)
        {
            try
            {
                var results = _postService.SearchPosts(_currentUserInfo.GetCurrentUserId(), keyword, language, tag);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("myposts")]
        [Authorize]
        public IActionResult GetMyPosts()
        {
            try
            {
                var userId = _currentUserInfo.GetCurrentUserId();
                var posts = _postService.GetUserPosts(userId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
