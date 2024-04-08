using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using hackathon_file_import.Core.Interfaces;
using hackathon_file_import.Core.Services;
using MongoDB.Bson;
using System.Collections.Generic;
using Newtonsoft.Json;
using SharpCompress.Common;

namespace hackathon_file_import.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileImportController : ControllerBase
    {

        private readonly IFileImportService _fileImportService;

        public FileImportController(IFileImportService fileImportService)
        {
            _fileImportService = fileImportService;
        }

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (!_fileImportService.IsValidFile(file))
            {
                return BadRequest("Invalid file type. Only CSV or Excel files are allowed.");
            }

            _fileImportService.SaveFile(file);
            return Ok("File uploaded successfully.");
        }
    }
}
