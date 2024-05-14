using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
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

        public async Task UpdateLibrary(Library library)
        {
            await _libraryRepository.UpdateLibrary(library);
        }

        public async Task DeleteLibrary(string name)
        {   
            await _libraryRepository.DeleteLibrary(name);

        }
    }
}
