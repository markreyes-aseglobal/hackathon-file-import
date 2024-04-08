using hackathon_file_import.Core.Interfaces;

namespace hackathon_file_import.Infrastructure.Repositories
{
    public class FileRepository
    {
        private readonly IMongoDbContext _context;

        public FileRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public void SaveFile(Stream fileStream, string fileName)
        {
            var fileBytes = new byte[fileStream.Length];
            fileStream.Read(fileBytes, 0, (int)fileStream.Length);
            _context.GridFS.UploadFromBytes(fileName, fileBytes);
        }
    }
}
