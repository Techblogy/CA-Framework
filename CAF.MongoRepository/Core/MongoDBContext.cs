using CAF.Core.Utilities;

using MongoDB.Driver;

namespace CAF.MongoRepository.Core
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }

        IMongoDatabase IMongoDBContext._db { get { return _db; } }

        MongoClient IMongoDBContext._mongoClient { get { return _mongoClient; } }

        public MongoDBContext(IAppSettings configuration)
        {
            _mongoClient = new MongoClient(configuration.MongoConnectionString);
            _db = _mongoClient.GetDatabase(configuration.MongoDatabaseName);

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
