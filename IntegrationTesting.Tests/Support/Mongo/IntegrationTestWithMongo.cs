using IntegrationTesting.API.Data.Mongo;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace IntegrationTesting.Tests.Support.Mongo
{
    public class IntegrationTestWithMongo : IntegrationTest, IDisposable
    {
        public readonly IMongoClient mongoClient;
        public readonly IMongoDatabase mongoDatabase;

        public IntegrationTestWithMongo(CustomWebAppFactory<API.Startup> httpTestFactory)
            : base(httpTestFactory)
        {
            var mongoConfiguration = serviceScope.ServiceProvider.GetRequiredService<IMongoConfiguration>();

            mongoClient = mongoConfiguration.GetClient();
            mongoDatabase = mongoConfiguration.GetDatabase();
        }

        public void Dispose()
        {
            mongoClient.DropDatabase(mongoDatabase.DatabaseNamespace.DatabaseName);
        }
    }
}
