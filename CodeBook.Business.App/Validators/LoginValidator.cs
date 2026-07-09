using System;
using FluentValidation;
using CodeBook.Business.App.DTOs;

namespace CodeBook.Business.App.Validator
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

        }
    }
}