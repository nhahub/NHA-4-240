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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AbstractValidator<LoginDto> _loginValidator;
        private readonly AbstractValidator<RegisterDto> _registerValidator;
        private readonly AbstractValidator<ResetPasswordDto> _resetPasswordValidator;
        private readonly AbstractValidator<ForgotPasswordDto> _forgotPasswordValidator;
        private readonly CurrentUserInfo _currentUserInfo;
        public AuthController(IAuthService authService, AbstractValidator<LoginDto> loginvalidator, AbstractValidator<RegisterDto> registervalidator, AbstractValidator<ResetPasswordDto> resetpasswordvalidator, AbstractValidator<ForgotPasswordDto> forgotpasswordvalidator, CurrentUserInfo currentUserInfo)
        {
            _authService = authService;
            _loginValidator = loginvalidator;
            _registerValidator = registervalidator;
            _resetPasswordValidator = resetpasswordvalidator;
            _forgotPasswordValidator = forgotpasswordvalidator;
            _currentUserInfo = currentUserInfo;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto logininfo)
        {
            var validationResult = _loginValidator.Validate(logininfo);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var response = _authService.Login(logininfo);
                if (response != null && response.Token != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true, //so JavaScript cannot access it
                        Secure = true, //secure it to https access
                        SameSite = SameSiteMode.None, //protection to not be accessed through header
                        Expires = DateTime.UtcNow.AddDays(7)

                    };
                    Response.Cookies.Append("jwt_token", response.Token, cookieOptions);

                    return Ok(new { message = "Login Successful.", Role = response.Role });
                }
                return Unauthorized(new { message = "Invalid Email or Password" });
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerinfo)
        {
            var validationResult = _registerValidator.Validate(registerinfo);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                ErrorResponse result = _authService.Register(registerinfo);
                if (result.Success)
                {
                    return Created(result.Message, registerinfo);
                }
                return Conflict(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("logout")]
        [Authorize]

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return Ok(new { message = "Logout Successful!" });
        }

        [HttpPatch("resetPassword")]
        [Authorize]
        public IActionResult ResetPassword([FromBody]ResetPasswordDto resetPassword)
        {
            var currentId = _currentUserInfo.GetCurrentUserId();
            resetPassword.userId = currentId;

            try
            {
                bool verifyold = _authService.VerifyPassword(resetPassword.password, resetPassword.userId);
                if (!verifyold)
                {
                    return BadRequest(new { message = "Incorrect Old Password" });
                }

                var validationResult = _resetPasswordValidator.Validate(resetPassword);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                ErrorResponse result = _authService.ResetPassword(resetPassword);

                if (result.Success)
                {
                    return Ok(new { message = result.Message });
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }
        [HttpPost("forgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var validationResult = _forgotPasswordValidator.Validate(forgotPassword);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                ErrorResponse result = _authService.ForgotPassword(forgotPassword);

                if (result.Success)
                {
                    return Ok(new { message = result.Message });
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
