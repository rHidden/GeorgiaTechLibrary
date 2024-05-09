using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
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
        public async Task<List<Book>> ListBooks()
        {
            var books = await _bookRepository.ListBooks();
            return books;
        }
        public async Task<Book> CreateBook(Book book)
        {
            var createdBook = await _bookRepository.CreateBook(book);
            return createdBook;
        }

        public async Task DeleteBook(int id)
        {
            await _bookRepository.DeleteBook(id);
        }

        public async Task<Book> GetBook(int Id)
        {
            var book = await _bookRepository.GetBook(Id);
            return book;
        }

        public async Task UpdateBook(Book book)
        {
            await _bookRepository.UpdateBook(book);
        }
    }
}
