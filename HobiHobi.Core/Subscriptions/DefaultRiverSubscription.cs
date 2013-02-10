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
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/nyt/River3.js")
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
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/movies/River3.js")
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
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/techmeme/River3.js")
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
                Text = "From the inventor of RSS",
                Name = "dave",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/iowaRiver3.js")
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
                JSONPUri = new Uri("http://static.scripting.com/podcastriver/River3.js")
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
                Text = "Apple",
                Name = "apple",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/apple/River3.js")
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

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "World",
                Name = "worldRiver",
                Language = "en",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/international/River3.js")
            });

            return river;
        }
    }
}
