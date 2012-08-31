using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Subscriptions;

namespace HobiHobi.Tests.Core.Feeds
{
    [TestFixture]
    public class RiverWallTests
    {
        [Test, Explicit]
        public void SaveRiverWall()
        {
            var wall = new RiverWall();
            wall.Id = RiverWall.NewId("dodyg").Full();
            wall.Name = "dodyg";
            wall.Template = DefaultRiverTemplate.Get();
            wall.Sources = DefaultRiverSubscription.Get();
            wall.Title = "Dody's Wall";

            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(wall);
                    session.SaveChanges();
                }
            }
        }

    }
}
