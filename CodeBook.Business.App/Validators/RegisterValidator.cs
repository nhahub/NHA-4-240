using System;
using FluentValidation;
using CodeBook.Business.App.DTOs;

namespace CodeBook.Business.App.Validator
{
	public class RegisterValidator : AbstractValidator<RegisterDto>
	{
		public RegisterValidator()
		{
            RuleFor(x  => x.UserName).NotEmpty().MaximumLength(50);
		    RuleFor(x => x.Password).NotEmpty().MaximumLength(12).Matches("[0-9]").WithMessage("Password must contain at least one number")
              .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
              .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
              .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
			RuleFor(x => x.Email).NotEmpty().EmailAddress();

    }
	}
}