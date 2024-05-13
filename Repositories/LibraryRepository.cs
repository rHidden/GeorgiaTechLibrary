using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;

namespace GeorgiaTechLibrary.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly GTLDbContext _context;

        public LibraryRepository(GTLDbContext context)
        {
            _context = context;
        }

        public Task<Library> CreateLibrary(Library library)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLibrary(string libraryName)
        {
            throw new NotImplementedException();
        }

        public Task<Library> GetLibrary(string libraryName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Library>> ListLibraries()
        {
            throw new NotImplementedException();
        }

        public Task UpdateLibrary(Library library)
        {
            throw new NotImplementedException();
        }

        private Library MapLibraryDTOToLibrary(LibraryDTO libraryDTO)
        {
            return new Library
            {
                Name = libraryDTO.Name,
                //address
            };
        }
    }
}
