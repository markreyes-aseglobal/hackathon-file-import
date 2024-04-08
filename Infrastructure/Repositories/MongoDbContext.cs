using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using hackathon_file_import.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Collections;

namespace hackathon_file_import.Infrastructure.Repositories
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB:ConnectionString");
            var databaseName = configuration["MongoDB:DatabaseName"];
            var collectionName = configuration["MongoDB:CollectionName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<BsonDocument>(collectionName);
        }
        public IGridFSBucket GridFS => new GridFSBucket(_database); 
    }
}
