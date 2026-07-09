using CodeBook.Data.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBook.Business.App.DTOs;
using CodeBook.Models.App;

namespace CodeBook.API.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly CodeBookContext _context;

        public TagController(CodeBookContext context)
        {
            _context = context;
        }

        [HttpPost("resolve")]
        [Authorize]
        public IActionResult ResolveTags([FromBody] ResolveTagRequest request)
        {
            try
            {
                var tagIds = new List<int>();
                foreach (var tagName in request.Tags)
                {
                    var tag = _context.tags.FirstOrDefault(t => t.Name.ToLower() == tagName.ToLower());
                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = tagName,
                            Slug = tagName.ToLower(),
                            DateCreated = DateTime.Now
                        };
                        _context.tags.Add(tag);
                        _context.SaveChanges();
                    }
                    tagIds.Add(tag.Id);
                }
                return Ok(new { tagIds = tagIds });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
