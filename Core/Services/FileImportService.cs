using hackathon_file_import.Core.Interfaces;
using hackathon_file_import.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace hackathon_file_import.Core.Services
{
    public class FileImportService : IFileImportService
    {
        private readonly IRepository<byte[]> _repository;
        private readonly IConfiguration _configuration;
        public FileImportService(IRepository<byte[]> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            var allowedFileTypes = _configuration.GetSection("AllowedFileTypes").Get<string[]>();
            if (!Array.Exists(allowedFileTypes, fileType => fileType.Equals(file.ContentType)))
            {
                return false;
            }

            return true;
        }

        public void SaveFile(IFormFile file, string userId)
        {
            using (var memoryStream = new MemoryStream())
            {
                var meta = new FileMetaData 
                {
                    FileName = file.FileName,
                    UserId = userId,
                    ContentType = file.ContentType
                };
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                _repository.Add(fileBytes, meta);
            }
        }

        public IEnumerable<FileMetaData> GetFileEntries()
        {
            return _repository.GetFileEntries();
        }
    }
}
