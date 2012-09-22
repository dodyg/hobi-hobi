using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using FluentValidation;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Validators;
using HobiHobi.Core.ViewModels;

namespace HobiHobi.Core
{
    public class BootStrap
    {
        public static void Register(ContainerBuilder builder, BootStrapType type = BootStrapType.Assembly)
        {
            //validators
            builder.RegisterType<UserViewModelValidator>().Keyed<IValidator>(typeof(UserViewModel));
            builder.RegisterType<RiverTemplateViewModelValidator>().Keyed<IValidator>(typeof(RiverTemplateViewModelValidator));
            builder.RegisterType<BasicRiverWallViewModelValidator>().Keyed<IValidator>(typeof(BasicRiverWallViewModel));
            builder.RegisterType<BasicSyndicationListViewModelValidator>().Keyed<IValidator>(typeof(BasicSyndicationListViewModel));
            builder.RegisterType<BasicBlogViewModelValidator>().Keyed<IValidator>(typeof(BasicBlogViewModel));
        }
    }
}
