using HobiHobi.Core.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.ViewModels
{
    public class RiverTemplateViewModel
    {
        public string RiverGuid { get; set; }
        public string RiverName { get; set; }
        public string RiverTitle { get; set; }
        public string LessCss { get; set; }
        public string JavaScript { get; set; }
        public string CoffeeScript { get; set; }
        public string FeedLiquidTemplate { get; set; }
        public string WallLiquidTemplate { get; set; }

        public RiverTemplateViewModel(RiverWall wall)
        {
            RiverGuid = wall.Guid;
            RiverName = wall.Name;
            RiverTitle = wall.Title;
            LessCss = wall.Template.LessCss.Text;
            JavaScript = wall.Template.JavaScript.Text;
            CoffeeScript = wall.Template.CoffeeScript.Text;
            FeedLiquidTemplate = wall.Template.FeedLiquidTemplate;
            WallLiquidTemplate = wall.Template.WallLiquidTemplate;
        }
    }
}
