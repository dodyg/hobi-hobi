using FluentValidation;
using HobiHobi.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Validators
{
    public class RiverTemplateViewModelValidator : AbstractValidator<RiverTemplateViewModel>
    {
        public RiverTemplateViewModelValidator()
        {
            RuleFor(m => m.WallLiquidTemplate).NotEmpty().WithMessage("Wall Template cannot be empty");
            RuleFor(m => m.FeedLiquidTemplate).NotEmpty().WithMessage("Feed Name cannot be empty");
            RuleFor(m => m.LessCss).NotEmpty().WithMessage("CSS is required");
            When(m => string.IsNullOrEmpty(m.JavaScript), () =>
                {
                    RuleFor(m => m.CoffeeScript).NotEmpty().WithMessage("CoffeeScript is required if you do not specify javascript");
                });

            When(m => string.IsNullOrEmpty(m.CoffeeScript), () =>
                {
                    RuleFor(m => m.JavaScript).NotEmpty().WithMessage("Javascript is required if you do not specify javascript");
                });
        }
    }
}
