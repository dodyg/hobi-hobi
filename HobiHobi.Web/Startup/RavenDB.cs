using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using System.Reflection;

namespace HobiHobi.Web.Startup
{
    public static class RavenDB
    {
        public static void Init(out DocumentStore store)
        {
            store = new DocumentStore { ConnectionStringName = "RavenDB" };
            store.Initialize();
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;

            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), store);

#if DEBUG
            store.DatabaseCommands.EnsureDatabaseExists(MvcApplication.DATABASE_NAME);
#endif

        }
    }
}