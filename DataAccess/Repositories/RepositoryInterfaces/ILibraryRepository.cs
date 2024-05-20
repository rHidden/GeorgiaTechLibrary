using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface ILibraryRepository
    {
        Task<Library> GetLibrary(string libraryName);
        Task<List<Library>> ListLibraries();
        Task<Library> CreateLibrary(Library library);
        Task<Library> UpdateLibrary(Library library);
        Task<bool> DeleteLibrary(string libraryName);
    }
}
