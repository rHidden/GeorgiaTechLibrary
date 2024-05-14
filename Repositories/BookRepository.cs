using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GeorgiaTechLibrary.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly GTLDbContext _context;

        public BookRepository(GTLDbContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateBook(Book book)
        {
            var bookEntity = new BookDTO
            {
                ISBN = book.ISBN,
                Description = book.Description,
                SubjectArea = book.SubjectArea,
                CanLoan = book.CanLoan
            };

            _context.Book.Add(bookEntity);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task DeleteBook(string ISBN)
        {
            try
            {
                var bookEntity = await _context.Book.FindAsync(ISBN);
                if (bookEntity != null)
                {
                    _context.Book.Remove(bookEntity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting book", e);
            }
        }

        public async Task<Book> GetBook(string ISBN)
        {
            var bookEntity = await _context.Book.FindAsync(ISBN);
            if (bookEntity != null)
            {
                return MapBookDTOToBook(bookEntity);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Book>> ListBooks()
        {
            var bookDTOs = await _context.Book.ToListAsync();
            return bookDTOs.Select(dto => MapBookDTOToBook(dto)).ToList();
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


        // Method to map BookDTO to Book
        private Book MapBookDTOToBook(BookDTO bookDTO)
        {
            return new Book
            {
                ISBN = bookDTO.ISBN.ToString(),
                Description = bookDTO.Description,
                SubjectArea = bookDTO.SubjectArea,
                CanLoan = bookDTO.CanLoan
            };
        }
    }
}
