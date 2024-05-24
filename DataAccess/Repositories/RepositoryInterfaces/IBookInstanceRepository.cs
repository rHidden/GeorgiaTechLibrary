using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IBookInstanceRepository
    {
        Task<BookInstance?> GetBookInstance(int id);
        Task<List<BookInstance>?> ListBookInstances();
        Task<BookInstance> CreateBookInstance(BookInstance bookInstance);
        Task<BookInstance> UpdateBookInstance(BookInstance bookInstance);
        Task<bool> DeleteBookInstance(int id);
    }
}
