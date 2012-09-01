using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public static class DefaultRiverTemplate
    {
        public static RiverTemplate Get()
        {
            var template = new RiverTemplate()
            {
                Id = RiverTemplate.NewId().Full(),
                Title = "Dark Wood",
                AuthorName = "Dody Gunawinata",
                FeedLiquidTemplate = _feedLiquidTemplate,
                WallLiquidTemplate = _wallLiquidTemplate,
                LessCss = new LessCssBundledText().SetText(_lessCss),
                JavaScript = new BundledText().SetText(_javascript),
                HtmlHeadInline = @"
<link href='http://fonts.googleapis.com/css?family=Yanone+Kaffeesatz:700' rel='stylesheet' type='text/css'>"
            };


            return template;
        }

        static string _javascript = @"
     $(function () {            
            $('#loading_div').hide()  // hide it initially
                .ajaxStart(function () {
                    $(this).show();
                })
                .ajaxStop(function () {
                $(this).hide();});
            
            var firstTab = $('#river_tabs li:first');
            firstTab.attr('class', 'active'); //select first one
            loadFromNavigation(firstTab.children(':first'));
            

            $('a[data-toggle=""tab""]').on('shown', function (e) {
                loadFromNavigation($(e.target));
            })
        });

        function loadFromNavigation(target) {
            var feedName = target.data('id');
            var feedOrigin = target.data('origin-feed');
            var feedTitle = target.text();
            load(feedName, feedTitle, false, feedOrigin);
        }

        function load(feedName, feedTitle, isInitial, feedOrigin) {
            
            $.get('/home/feed?feedname=' + feedName, function (data) {
                //change title
                $('#river_title').html(feedTitle);

                if (feedOrigin !== undefined) {
                    $('#river_origin_feed')
                        .attr('href', 'http://' + feedOrigin)
                        .html(feedOrigin);
                }

                $('#river').html(data);
                if (isInitial)
                    arrange();
                else {
                    $('#river').isotope('destroy');
                    arrange() 
                }
            });
        }

        function arrange() {
            $('#river').isotope({
                itemSelector: '.feed_item',
                layoutMode: 'masonry',
                getSortData: {
                    feed_id: function ($elem) {
                        return parseInt($elem.data('id'));
                    }
                },
                sortBy: 'feed_id',
                sortAscending: false
            });
        }
";

        static string _lessCss = @"
body {
            background-image: url('/images/dark_wood.png');
            background-repeat: repeat;
                
            /*background-color:#4d6a80;*/
            color:lightgray;
        }

        h1, h2 {
            color: lightgray;
            font-family: 'Yanone Kaffeesatz', sans-serif;
        }
        h1 {
            font-size:48px;
        }

        h2 {
            font-size:26px;
            line-height:28px;
        }

        a:link, a:visited{
            color:lightgray;
        }
        
        .feed_item {
            width: 300px;
            
            margin-right:25px;
            margin-top:5px;
            margin-bottom:25px;
        }

            
            .feed_item h3 {
                font-size: 14px;
                line-height: 16px;
            }

            a.item_link {
                color:deeppink;
            }

        .feed_item_body {
            background-image: url('/images/paper.png');
            background-repeat: repeat;
            background-color:#f7f1f1;
            padding:5px;
            border-bottom-right-radius: 12px;
            border-top-right-radius: 12px; 
            font-family:'Courier New';
            color: #232323;
        }

        .feed_item_body a:link, .feed_item_body a:visited{
            color:deeppink;
        }

        .feed_item_thumbnail {
            margin:5px;
        }
        .feed_item_comments, .feed_item_date, .last_updated, .feed_origin_website, .feed_origin_website a:link {
            font-size:10px;
        }
       
        
        #loading_div {
            height: 400px;
            position: relative;
        }
            #loading_div img{
                position: absolute;
                left: 50%;
                top: 50%;
                margin-left: -32px; /* -1 * image width / 2 */
                margin-top: -32px;  /* -1 * image height / 2 */
                display: block;     
            }

";

        static string _wallLiquidTemplate = @"
<div class=""tabbable"">
    <ul class=""nav nav-tabs"" id=""river_tabs"">
        {% for river in rivers -%}
            <li><a href=""#{{ river.name }}"" data-id=""{{ river.name }}"" data-toggle=""tab"" data-origin-feed=""{{ river.json_p }}"">{{ river.text }}</a></li>
        {% endfor -%}
    </ul>
</div>

<h1 id=""river_title""></h1>
<br />

<div style=""font-size:16px;"">
Source : <a href=""#"" id=""river_origin_feed""></a>
</div>


<div id=""river"">
</div>
<br /><br />

<div id=""loading_div"">
    <img src=""/images/ajax_spinner.gif"" alt=""wait"" />
</div>

";
        static string _feedLiquidTemplate = @"
{% for feed in feeds -%}
        {% for item in feed.items -%}
            <div class=""feed_item"" data-id=""{{ item.id }}"">
                <h2>{{ item.title }} <a href=""{{ item.link }}"" class=""item_link"">#</a></h2>
                <div class=""feed_item_body"">
                    {% if item.thumbnails -%}
                    <div class=""feed_item_thumbnail"">
                        {% for thumb in item.thumbnails -%}
                            <img src=""{{ thumb.url }}"" width=""{{ thumb.width }}"" height=""{{ thumb.height }}"" />
                        {% endfor -%}
                    </div><!-- end of feed_item_thumbnail -->
                    {% endif -%}
                    {{ item.body }}
                    <p class=""feed_item_date"">{{ item.pub_date }}</p>
                    {% if feed.title != """" -%}
                    <div class=""feed_origin_website"">Source: <a href=""{{ feed.website_url }}"">{{ feed.title }}</a></div>
                    {% else %}
                    <div class=""feed_origin_website""><a href=""{{ feed.website_url }}"">Source</a></div>
                    {% endif -%}
                    {% if item.comments_link -%}
                    <div class=""feed_item_comments""><a href=""{{ item.comments_link }}"">Comments</a></div> 
                    {% endif -%}
                </div><!-- feed item body-->
            </div><!-- feed_item -->
        {% endfor -%}
{% endfor -%}
";
    }
}
