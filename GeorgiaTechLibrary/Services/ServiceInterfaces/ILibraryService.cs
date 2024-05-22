using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface ILibraryService
    {
        Task<Library> GetLibrary(string name);
        Task<List<Library>> ListLibraries();
        Task<Library> CreateLibrary(Library library);
        Task<Library> UpdateLibrary(Library library);
        Task<bool> DeleteLibrary(string name);

    }
}
