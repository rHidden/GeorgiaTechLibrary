using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBook(string ISBN);
        Task<List<Book>> ListBooks();
        Task<Book> CreateBook(Book book);
        Task UpdateBook(Book book);
        Task<Book> DeleteBook(string ISBN);
    }
}
