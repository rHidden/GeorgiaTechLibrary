using DataAccess.DAO;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;

namespace DataAccess.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly GTLDbContext _context;

        public LibraryRepository(GTLDbContext context)
        {
            _context = context;
        }
        public Task<Library> GetLibrary(string libraryName)
        {
            throw new NotImplementedException();
        }
        public Task<List<Library>> ListLibraries()
        {
            throw new NotImplementedException();
        }

        public Task<Library> CreateLibrary(Library library)
        {
            throw new NotImplementedException();
        }
        public Task UpdateLibrary(Library library)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLibrary(string libraryName)
        {
            throw new NotImplementedException();
        }

    }
}
