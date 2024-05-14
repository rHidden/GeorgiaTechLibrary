using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateBook(Book book);
        Task<Book> GetBook(string ISBN);
        Task UpdateBook(Book book);
        Task<Book> DeleteBook(string ISBN);
        Task<List<Book>> ListBooks();
    }
}
