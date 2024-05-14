using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IBookService
    {
        Task<Book> GetBook(string ISBN);
        Task<Book> CreateBook(Book book);
        Task UpdateBook(Book book);
        Task<Book> DeleteBook(string ISBN);
        Task<List<Book>> ListBooks();
    }
}
