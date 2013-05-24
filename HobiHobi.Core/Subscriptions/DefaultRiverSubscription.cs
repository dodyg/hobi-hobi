using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    public static class DefaultRiverSubscription
    {
        public static RiverSubscription Get()
        {
            var river = new RiverSubscription();
            river.Title = "Default Rivers";
            river.OwnerName = "Hobi Hobi";
            river.DateCreated = Stamp.Time();
            river.DateModified = Stamp.Time();

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "New York Times",
                Name = "nyt",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/river3/rivers/nytRiver.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "The Economist's Blogs",
                Name = "theEconomistBlogs",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/the-economist-blogs")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Magazines",
                Name = "magazines",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/magazines")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Movies",
                Name = "moviesRiver",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/river3/rivers/movies.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Reuters' Market News",
                Name = "ReutersMarkets",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/reuters-markets")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Technology",
                Name = "tech",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/river3/rivers/tech.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Celebrities",
                Name = "celebrities",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/celebrities")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Dave Winer",
                Name = "dave",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/river3/rivers/iowa.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "No Agenda Network",
                Name = "noagenda",
                Language = "en",
                JSONPUri = new Uri("http://s3.amazonaws.com/river.curry.com/rivers/radio2/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Podcasts",
                Name = "podcastRiver",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/river3/rivers/podcasts.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Nouvelles Marocaine",
                Name = "marocainNovelles",
                Language = "fr",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/marocain-nouvelles")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Notizie Italia",
                Name = "notizieItalia",
                Language = "it",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/Italian")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Programming",
                Name = "programmingNews",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/programming")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "ASEAN Nations",
                Name = "asean",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/asean")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Southern Africa",
                Name = "asean",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/southern-africa")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Middle East",
                Name = "middleEast",
                Language = "en",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/middle-east")
            });

            
            return river;
        }
    }
}
