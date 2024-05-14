using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IBookService
    {
        Task<Book> GetBook(string ISBN);
        Task<List<Book>> ListBooks();
        Task<Book> CreateBook(Book book);
        Task UpdateBook(Book book);
        Task<Book> DeleteBook(string ISBN);
    }
}
