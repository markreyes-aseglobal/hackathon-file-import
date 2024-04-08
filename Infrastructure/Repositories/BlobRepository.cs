using hackathon_file_import.Core.Interfaces;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;

namespace hackathon_file_import.Infrastructure.Repositories
{
    public class BlobRepository : IRepository<byte[]>
    {
        private readonly GridFSBucket _gridFS;

        public BlobRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ASEHackathon");
            _gridFS = new GridFSBucket(database);
        }

        public void Add(byte[] entity)
        {
            _gridFS.UploadFromBytes("files", entity);
        }
    }
}
