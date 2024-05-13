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

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task DeleteBook(string ISBN)
        {
            var bookEntity = await _context.Books.FindAsync(ISBN);
            if (bookEntity != null)
            {
                _context.Books.Remove(bookEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> GetBook(string ISBN)
        {
            var bookEntity = await _context.Books.FindAsync(ISBN);
            if (bookEntity != null)
            {
                return new Book
                {
                    ISBN = bookEntity.ISBN,
                    Description = bookEntity.Description,
                    SubjectArea = bookEntity.SubjectArea,
                    CanLoan = bookEntity.CanLoan
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Book>> ListBooks()
        {
            var bookDTOs = await _context.Books.ToListAsync();
            return bookDTOs.Select(dto => MapBookDTOToBook(dto)).ToList();
        }

        public async Task UpdateBook(Book book)
        {
            var bookEntity = await _context.Books.FindAsync(book.ISBN);
            if (bookEntity != null)
            {
                bookEntity.Description = book.Description;
                bookEntity.SubjectArea = book.SubjectArea;
                bookEntity.CanLoan = book.CanLoan;

                await _context.SaveChangesAsync();
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
