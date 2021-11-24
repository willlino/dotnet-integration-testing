using IntegrationTesting.API.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Net.Http;
using Xunit;

namespace IntegrationTesting.Tests.Support
{
    public class IntegrationTest : IClassFixture<CustomWebAppFactory<API.Startup>>, IDisposable
    {
        public readonly HttpClient httpClient;
        public readonly IMongoClient mongoClient;
        public readonly IMongoDatabase mongoDatabase;

        public IntegrationTest(CustomWebAppFactory<API.Startup> httpTestFactory)
        {
            var _httpTestFactory = httpTestFactory;

            httpClient = _httpTestFactory.CreateClient();
            var scope = _httpTestFactory.Services.CreateScope();
            var mongoConfiguration = scope.ServiceProvider.GetRequiredService<IMongoConfiguration>();

            mongoClient = mongoConfiguration.GetClient();
            mongoDatabase = mongoConfiguration.GetDatabase();
        }

        public void Dispose()
        {
            mongoClient.DropDatabase(mongoDatabase.DatabaseNamespace.DatabaseName);
        }
    }
}
