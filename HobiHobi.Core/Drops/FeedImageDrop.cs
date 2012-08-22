using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using HobiHobi.Core.Feeds;

namespace HobiHobi.Core.Drops
{
    public class FeedImageDrop : Drop
    {
        public string Url { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public FeedImageDrop(FeedImage image)
        {
            Url = image.Url;
            Width = image.Width;
            Height = image.Height;
        }
    }
}
