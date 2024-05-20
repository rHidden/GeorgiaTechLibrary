using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<Book> GetBook(string ISBN)
        {
            return await _bookRepository.GetBook(ISBN);
        }
        public async Task<List<Book>> ListBooks()
        {
            return await _bookRepository.ListBooks();
        }
        public async Task<Book> CreateBook(Book book)
        {
            return await _bookRepository.CreateBook(book);
        }
        public async Task<Book> UpdateBook(Book book)
        {
            return await _bookRepository.UpdateBook(book);
        }
        public async Task<bool> DeleteBook(string ISBN)
        {
            return await _bookRepository.DeleteBook(ISBN);
        }
    }
}
