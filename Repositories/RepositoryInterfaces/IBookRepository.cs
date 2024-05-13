using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateBook(Book book);
        Task<Book> GetBook(int ISBN);
        Task UpdateBook(Book book);
        Task DeleteBook(int ISBN);
        Task<List<Book>> ListBooks();
    }
}
