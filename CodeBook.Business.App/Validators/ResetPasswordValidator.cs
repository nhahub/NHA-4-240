using CodeBook.Business.App.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.newPassword).NotEmpty().MaximumLength(12).Matches("[0-9]").WithMessage("Password must contain at least one number")
              .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
              .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
              .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
              .NotEqual(x => x.password);
        }
    }
}
