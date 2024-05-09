using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBook(int ISBN);
        Task<Book> CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int ISBN);
        Task<List<Book>> ListBooks();
    }
}
