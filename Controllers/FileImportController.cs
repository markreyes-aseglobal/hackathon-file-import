using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using hackathon_file_import.Core.Interfaces;

namespace hackathon_file_import.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileImportController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        public FileImportController(IFileRepository fileRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var allowedFileTypes = _configuration.GetSection("AllowedFileTypes").Get<string[]>();


            // Check if the uploaded file's content type is allowed
            if (!Array.Exists(allowedFileTypes, fileType => fileType.Equals(file.ContentType)))
            {
                return BadRequest("Invalid file type. Please upload an allowed file type.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    _fileRepository.SaveFile(stream, file.FileName);
                }
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the file.");
            }
        }

    }
}
