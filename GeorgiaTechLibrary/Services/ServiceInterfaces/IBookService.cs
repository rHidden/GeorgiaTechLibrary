using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IBookService
    {
        Task<Book> GetBook(string ISBN);
        Task<List<Book>> ListBooks();
        Task<Book> CreateBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(string ISBN);
        Task<List<Book>> GetMostPopularBooksAmongStudents();
    }
}
