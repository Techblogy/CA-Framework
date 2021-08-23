using MongoDB.Driver;

namespace CAF.MongoRepository.Core
{
    public interface IMongoDBContext
    {
        IMongoDatabase _db { get; }
        MongoClient _mongoClient { get; }
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
