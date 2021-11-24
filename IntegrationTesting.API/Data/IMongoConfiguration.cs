using MongoDB.Driver;

namespace IntegrationTesting.API.Data
{
    public interface IMongoConfiguration
    {
        IMongoClient GetClient();
        IMongoDatabase GetDatabase();
    }
}