using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace IntegrationTesting.API.Data
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public readonly IMongoClient mongoClient;
        public readonly IMongoDatabase mongoDatabase;

        public MongoConfiguration(IConfiguration configuration)
        {
            this.mongoClient = new MongoClient(configuration.GetConnectionString("Mongo"));
            this.mongoDatabase = mongoClient.GetDatabase(configuration.GetConnectionString("MongoDatabaseName"));
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
