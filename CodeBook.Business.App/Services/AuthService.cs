using BCrypt.Net;
using CodeBook.Business.App.DTOs;
using CodeBook.Business.App.Interfaces;
using CodeBook.Business.App.Middleware;
using CodeBook.Business.App.Services;
using CodeBook.Data.App.IRepositories;
using CodeBook.Models.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeBook.Business.App.Methods

{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public AuthService(IUserRepository userRepository,IConfiguration configuration)
		{
			_userRepository = userRepository;
            _configuration = configuration;
		}

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"];
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("password_version", user.PasswordHash.Substring(0, 10))
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public LoginResponse Login(LoginDto login)
		{
            User existinguser = _userRepository.GetProfileByEmail(login.Email);
			if (existinguser == null)
				return null;

            bool found = BCrypt.Net.BCrypt.Verify(login.Password,existinguser.PasswordHash);
			if(!found) return null;
            var loginrespone = new LoginResponse();
            loginrespone.Role = existinguser.Role.ToString();
            loginrespone.Token = GenerateJwtToken(existinguser);
            return loginrespone;

		}
		public ErrorResponse Register(RegisterDto register)
		{
			User existinguser = _userRepository.GetProfileByEmail(register.Email);
            if (existinguser != null) return new ErrorResponse { Success = false, Message = "Aleady Registerd, Just login!" };
            
            User user = new User();
            user.Email = register.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);
			user.UserName = register.UserName;
			user.Role = UserRole.NormalUser;

			_userRepository.Add(user);
			if(_userRepository.SaveChanges()) return new ErrorResponse { Success = true, Message = "Registeration Success!" };
            return new ErrorResponse { Success = false, Message = "Couldn't Register!" };
        }

		public ErrorResponse ResetPassword(ResetPasswordDto resetPassword)
		{
			if (resetPassword == null) return new ErrorResponse { Success = false, Message = "Please fill the required fields!" };
			User user = _userRepository.GetProfileById(resetPassword.userId);
			if (user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" };
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPassword.newPassword);
            _userRepository.Update(user);
			if(_userRepository.SaveChanges()) return new ErrorResponse { Success = true, Message = "Password Reset Successfully!" };
            return new ErrorResponse { Success = false, Message = "Couldn't Reset Password!" };
        }
        public ErrorResponse ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            if (forgotPassword == null) return new ErrorResponse { Success = false, Message = "Please fill the required fields!" };
            User user = _userRepository.GetProfileByEmail(forgotPassword.email);
            if (user == null) return new ErrorResponse { Success = false, Message = "User Not Found!" };
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(forgotPassword.newPassword);
            _userRepository.Update(user);
            if (_userRepository.SaveChanges()) return new ErrorResponse { Success = true, Message = "Password Reset Successfully!" };
            return new ErrorResponse { Success = false, Message = "Couldn't Reset Password!" };
        }


        public bool VerifyPassword(string password, int userId)
        {
            User user = _userRepository.GetProfileById(userId);

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

    }
}