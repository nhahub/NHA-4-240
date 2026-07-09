using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Middleware;
using CodeBook.Models.App;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CodeBook.Business.App.Interfaces
{
    public interface IAuthService
    {
        ErrorResponse Register(RegisterDto register);
        LoginResponse Login(LoginDto login);
        ErrorResponse ResetPassword(ResetPasswordDto resetPassword);
        bool VerifyPassword(string password, int userId);
        public ErrorResponse ForgotPassword(ForgotPasswordDto forgotPassword);

    }
}