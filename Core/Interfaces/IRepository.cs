using hackathon_file_import.Core.Models;

namespace hackathon_file_import.Core.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);

        IEnumerable<FileMetaData> GetFileEntries();
    }
}
