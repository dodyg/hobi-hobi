using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using HobiHobi.Core.ViewModels;

namespace HobiHobi.Core.Validators
{
    public class BasicSyndicationListViewModelValidator : AbstractValidator<BasicSyndicationListViewModel>
    {
        public BasicSyndicationListViewModelValidator()
        {
            RuleFor(m => m.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(m => m.Name).Length(4, 40).WithMessage("Name must be between 4 to 40 characters");
            RuleFor(m => m.Name).Must(x => {
                return !string.IsNullOrWhiteSpace(x) && !x.Contains(" ") && !x.Contains("#") && !x.Contains("!") && !x.Contains("/");
            }).WithMessage("Name must not contains a whitespace or # or ! or /");

        }
    }
}
