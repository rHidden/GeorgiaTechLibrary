using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibraryService(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Library> GetLibrary(string name)
        {
            return await _libraryRepository.GetLibrary(name);
        }

        public async Task<List<Library>> ListLibraries()
        {
            return await _libraryRepository.ListLibraries();
        }

        public async Task<Library> CreateLibrary(Library library)
        {
            return await _libraryRepository.CreateLibrary(library);
        }

        public async Task<Library> UpdateLibrary(Library library)
        {
            return await _libraryRepository.UpdateLibrary(library);
        }

        public async Task<bool> DeleteLibrary(string name)
        {   
            return await _libraryRepository.DeleteLibrary(name);
        }
    }   
}
