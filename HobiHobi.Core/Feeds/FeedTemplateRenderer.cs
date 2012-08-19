using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotLiquid;
using System.Diagnostics;
using HobiHobi.Core.Drops;

namespace HobiHobi.Core.Feeds
{
    /// <summary>
    /// Render a given river data structure to string using dotliquid
    /// </summary>
    public class FeedTemplateRenderer
    {
        FeedsRiver _river;
        string _template;
        public FeedTemplateRenderer(FeedsRiver river, string template)
        {
            Debug.Assert(river != null);
            Debug.Assert(!String.IsNullOrWhiteSpace(template));

            _river = river;
            _template = template;
        }

        /// <summary>
        /// Render json river structure to html string 
        /// </summary>
        /// <returns></returns>
        public HtmlString Render()
        {
            Template tmplt = Template.Parse(_template);

            string result = tmplt.Render(Hash.FromAnonymousObject(
                new
                {
                    Feeds = _river.UpdatedFeeds.UpdatedFeed.Select(x => new FeedSiteDrop(x)).ToList()
                }
                ));

            return new HtmlString(result);
        }
    }
}
