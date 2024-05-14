using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface ILibraryRepository
    {
        Task<Library> GetLibrary(string libraryName);
        Task<List<Library>> ListLibraries();
        Task<Library> CreateLibrary(Library library);
        Task UpdateLibrary(Library library);
        Task DeleteLibrary(string libraryName);
    }
}
