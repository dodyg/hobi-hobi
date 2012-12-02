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
                Text = "NYT",
                Name = "nyt",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/nyt/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Dave",
                Name = "dave",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/iowaRiver3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "No Agenda",
                Name = "noagenda",
                JSONPUri = new Uri("http://s3.amazonaws.com/river.curry.com/rivers/radio2/River3.js")
            });


            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Podcasts",
                Name = "podcastRiver",
                JSONPUri = new Uri("http://static.scripting.com/podcastriver/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Movies",
                Name = "moviesRiver",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/movies/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Apple",
                Name = "apple",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/apple/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Programming",
                Name = "programmingNews",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/programming")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Tech Meme",
                Name = "tech",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/techmeme/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "ASEAN Nations",
                Name = "asean",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/asean")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Southern Africa",
                Name = "asean",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/southern-africa")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "World",
                Name = "worldRiver",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/international/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Nomadlife",
                Name = "nomadlife",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/nomadlife")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "East Village News",
                Name = "eastVillageNews",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/eastVillageRiver3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Magazines",
                Name = "magazines",
                JSONPUri = new Uri("http://hobieu.apphb.com/s/riverjs/magazines")
            });

            return river;
        }
    }
}
