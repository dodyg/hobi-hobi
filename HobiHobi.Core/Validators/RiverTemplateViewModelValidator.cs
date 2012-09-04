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
            RuleFor(m => m.WallTemplate).NotEmpty().WithMessage(Resources.RiverTemplateViewModelValidator.ReqWallTemplate);
            RuleFor(m => m.FeedTemplate).NotEmpty().WithMessage(Resources.RiverTemplateViewModelValidator.ReqFeedTemplate);
            RuleFor(m => m.LessCss).NotEmpty().WithMessage(Resources.RiverTemplateViewModelValidator.ReqCss);
            RuleFor(m => m.JavaScript).NotEmpty().WithMessage(Resources.RiverTemplateViewModelValidator.ReqJavaScript);
        }
    }
}
