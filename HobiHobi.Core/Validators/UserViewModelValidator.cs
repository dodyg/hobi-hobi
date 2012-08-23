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

        }
    }
}
