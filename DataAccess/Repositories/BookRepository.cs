using DataAccess.DAO;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly GTLDbContext _context;

        public BookRepository(GTLDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetBook(string ISBN)
        {
            var bookEntity = await _context.Book.FindAsync(ISBN);
            if (bookEntity != null)
            {
                return bookEntity;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Book>> ListBooks()
        {
            var bookDTOs = await _context.Book.ToListAsync();
            return bookDTOs.ToList();
        }

        public async Task<Book> CreateBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task UpdateBook(Book book)
        {
            try
            {
                var bookEntity = await _context.Book.FindAsync(book.ISBN);
                if (bookEntity != null)
                {
                    _context.Entry(bookEntity).CurrentValues.SetValues(book); // Update entity properties

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Concurrency conflict occurred while updating the book.", ex);
            }
        }

        public async Task<Book> DeleteBook(string ISBN)
        {
            var foundBook = await _context.Book.FirstAsync(x => x.ISBN == ISBN);

            if (foundBook == null)
            {
                return null;
            }

            _context.Book?.Remove(foundBook);

            await _context.SaveChangesAsync();

            return foundBook;
        }
    }
}
