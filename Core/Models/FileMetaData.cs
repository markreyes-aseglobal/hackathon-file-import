using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace hackathon_file_import.Core.Models
{
    public class FileMetaData
    {
        public ObjectId Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string UserId { get; set; }
        public string ContentType { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
    }
}
