using hackathon_file_import.Core.Interfaces;
using hackathon_file_import.Infrastructure.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using hackathon_file_import.Core.Models;

namespace hackathon_file_import.Infrastructure.Repositories
{
    public class BlobRepository : IRepository<byte[]>
    {
        private readonly GridFSBucket _gridFS;

        public BlobRepository(MongoDbContext dbContext)
        {
            _gridFS = new GridFSBucket(dbContext.Database, new GridFSBucketOptions
            {
                BucketName = "fs",
                ChunkSizeBytes = 1048576, // 1MB chunk size
                DisableMD5 = false,
                WriteConcern = WriteConcern.WMajority
            });

            
        }

        public void Add(byte[] entity)
        {
            FileMetaData meta = new FileMetaData
            {
                UserId = "1234",
                ContentType = "application/csv"
            };

            var metadata = new BsonDocument
            {
                { "userId", new BsonString(meta.UserId) }, 
                { "contentType", new BsonString(meta.ContentType) },
                { "isDeleted", new BsonBoolean(false) },
                { "deletedBy", BsonNull.Value },
                { "deletedDate", BsonNull.Value }
            };


            _gridFS.UploadFromBytes("files", entity, new GridFSUploadOptions
            {
                Metadata = metadata
            });
        }

        public IEnumerable<FileMetaData> GetFileEntries()
        {
            var filter = Builders<GridFSFileInfo>.Filter.Empty;
            var options = new GridFSFindOptions
            {
                BatchSize = 10, // Adjust batch size as needed
                Sort = Builders<GridFSFileInfo>.Sort.Descending(f => f.UploadDateTime) // Sort by upload date
            };

            var fileEntries = _gridFS.FindAsync(filter, options).Result.ToList();

            var result = new List<FileMetaData>();
            foreach (var fileInfo in fileEntries)
            {
                result.Add(new FileMetaData
                {
                    FileName = fileInfo.Filename,
                    UserId = fileInfo.Metadata["userid"].AsString,
                    ContentType = fileInfo.Metadata["extension"].AsString,
                    IsDeleted = fileInfo.Metadata["isdeleted"].AsBoolean
                    // Handle DeletedBy and DeletedDate if needed
                });
            }

            return result;
        }
    }
}
