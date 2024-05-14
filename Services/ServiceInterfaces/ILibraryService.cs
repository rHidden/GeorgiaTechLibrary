using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILibraryService
    {
        Task<Library> GetLibrary(string name);
        Task<List<Library>> ListLibraries();
        Task<Library> CreateLibrary(Library library);
        Task UpdateLibrary(Library library);
        Task DeleteLibrary(string name);

    }
}
