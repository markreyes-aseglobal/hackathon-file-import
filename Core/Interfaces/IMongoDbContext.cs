namespace hackathon_file_import.Core.Interfaces
{
    using MongoDB.Driver.GridFS;

    public interface IMongoDbContext
    {
        IGridFSBucket GridFS { get; }
    }
}
