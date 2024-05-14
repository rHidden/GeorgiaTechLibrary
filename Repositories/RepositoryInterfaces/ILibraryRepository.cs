using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface ILibraryRepository
    {
        Task<Library> CreateLibrary(Library library);
        Task UpdateLibrary(Library library);
        Task<Library> GetLibrary(string libraryName);
        Task DeleteLibrary(string libraryName);
        Task<List<Library>> ListLibraries();
    }
}
