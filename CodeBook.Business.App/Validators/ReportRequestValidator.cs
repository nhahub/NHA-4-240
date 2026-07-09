using CodeBook.Business.App.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.Validators
{
    public class ReportRequestValidator : AbstractValidator<ReportRequest>
    {
        public ReportRequestValidator() {
            RuleFor(r => r.Reason).NotEmpty();
        }
    }
}
