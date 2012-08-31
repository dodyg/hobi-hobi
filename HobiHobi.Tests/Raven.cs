using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;

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
    }
}
