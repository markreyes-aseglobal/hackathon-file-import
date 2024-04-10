﻿using hackathon_file_import.Core.Models;
using Microsoft.AspNetCore.Http;

namespace hackathon_file_import.Core.Interfaces
{
    public interface IFileImportService
    {
        bool IsValidFile(IFormFile file);
        void SaveFile(IFormFile file);

        IEnumerable<FileMetaData> GetFileEntries();
    }
}
