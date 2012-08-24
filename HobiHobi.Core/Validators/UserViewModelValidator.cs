using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using HobiHobi.Core.ViewModels;

namespace HobiHobi.Core.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(Resources.UserViewModelValidator.ReqFirstName);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(Resources.UserViewModelValidator.ReqLastName);
            RuleFor(m => m.Email).NotEmpty().WithMessage(Resources.UserViewModelValidator.ReqEmail);

            When((m => string.IsNullOrWhiteSpace(m.Id)), () =>
            {
                RuleFor(m => m.Password).NotEmpty().WithMessage(Resources.UserViewModelValidator.ReqPassword);
                RuleFor(m => m.RepeatPassword).NotEmpty().WithMessage(Resources.UserViewModelValidator.ReqRepeatPassword);
                RuleFor(m => m.Password).Must((m, password) => password == m.RepeatPassword).WithMessage(Resources.UserViewModelValidator.MatchPassword);
            });
        }
    }
}
