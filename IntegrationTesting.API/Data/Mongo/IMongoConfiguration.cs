using MongoDB.Driver;

namespace IntegrationTesting.API.Data.Mongo
{
    public interface IMongoConfiguration
    {
        IMongoClient GetClient();
        IMongoDatabase GetDatabase();
    }
}