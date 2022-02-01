using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System;
using IntegrationTesting.API.Data.Mongo;

namespace IntegrationTesting.Tests.Support.Mongo
{
    public class MongoTestConfiguration : IMongoConfiguration
    {
        public readonly IMongoClient mongoClient;
        public readonly IMongoDatabase mongoDatabase;

        public MongoTestConfiguration(IConfiguration configuration)
        {
            this.mongoClient = new MongoClient(configuration.GetConnectionString("Mongo"));

            var configDatabaseName = configuration.GetConnectionString("MongoDatabaseName");

            ValidateMongoTestConnectionString(configuration, configDatabaseName);

            this.mongoDatabase = mongoClient.GetDatabase($"{configDatabaseName}-{Guid.NewGuid()}");
        }

        private static void ValidateMongoTestConnectionString(IConfiguration configuration, string configDatabaseName)
        {
            var databaseTestName = "TestDatabase";
            if (configDatabaseName != databaseTestName)
                throw new Exception($"The test database name is invalid, and should be {databaseTestName}");
        }

        public IMongoClient GetClient()
        {
            return mongoClient;
        }

        public IMongoDatabase GetDatabase()
        {
            return mongoDatabase;
        }
    }
}
