using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Framework;

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
            wall.Guid = Stamp.GUID().ToString();
            wall.Name = "dodyg";
            wall.Template = DefaultRiverTemplate.Get();
            wall.Sources = DefaultRiverSubscription.Get();
            wall.Title = "Dody's Wall";
            wall.Description = "This is Dody Gunawinata's Wall";
            wall.Keywords = "dody, gunawinata, awesome";

            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    session.Store(wall);
                    session.SaveChanges();
                }
            }
        }

        [Test, Explicit]
        public void LoadRiverWall()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var river = session.Load<RiverWall>(RiverWall.NewId("dodyg").Full());
                    Assert.IsNotNull(river, "River cannot be null");
                }
            }
        }

    }
}
