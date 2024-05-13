using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface ILibraryRepository
    {
        Task<Library> CreateLibrary(Library library);
        Task UpdateLibrary(Library library);
        Task<Library> GetLibrary(string libraryName);
        Task DeleteLibrary(string libraryName); //Will it be possible to just remove a library?
        Task<List<Library>> ListLibraries(); //Probably not necessary - maybe only if the user wants to see all libraries?
    }
}
