namespace hackathon_file_import.Core.Interfaces
{
    using System.IO;
    public interface IFileRepository
    {
        void SaveFile(Stream fileStream, string fileName);
    }
}
