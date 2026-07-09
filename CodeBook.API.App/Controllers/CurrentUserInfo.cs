using CodeBook.Models.App;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeBook.API.App.Controllers
{
    public class CurrentUserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetCurrentUserId()
        {
            if(_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
            {
                throw new UnauthorizedAccessException();
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int currentid))
            {
                return currentid;
            }
            throw new UnauthorizedAccessException();
        }
    }
}
