using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client;

namespace HobiHobi.Tests
{
    public static class Raven
    {
        public static DocumentStore GetStoreFromServer()
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDB" };
            store.Initialize();
            return store;
        }

        public static void Session(Action<IDocumentSession> action)
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    action(session);
                }
            }
        }

        public const string DATABASE_NAME = "hobihobi";
    }
}
