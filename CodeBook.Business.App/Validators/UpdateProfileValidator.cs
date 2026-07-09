using System;
using FluentValidation;
using CodeBook.Business.App.DTOs;

namespace CodeBook.Business.App.Validator
{
	public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
	{
		public UpdateProfileValidator() 
		{
			RuleFor(x  => x.UserName).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Bio).MaximumLength(500);
			RuleFor(x => x.AvatarUrl).Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.AvatarUrl));

        }
    }
}