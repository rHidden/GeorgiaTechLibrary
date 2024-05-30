using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("{ISBN}")]
        [SwaggerOperation(Summary = "Get book",
            Description = "Returns a book based on the passed ISBN.\n\n" +
            "param ISBN - International Standard Book Number")]
        public async Task<IActionResult> GetBookAsync(string ISBN)
        {
            Book book = await _bookService.GetBook(ISBN);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("description/{ISBN}")]
        [SwaggerOperation(Summary = "Get book description",
            Description = "Returns the description of a book based on the passed ISBN.\n\n" +
            "param ISBN - International Standard Book Number")]
        public async Task<IActionResult> GetBookDescriptionAsync(string ISBN)
        {
            Book book = await _bookService.GetBook(ISBN);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book.Description);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all books",
            Description = "Returns a list of all books.")]
        public async Task<IActionResult> ListBooksAsync()
        {
            List<Book> books = await _bookService.ListBooks();
            if (!books.Any())
            {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new book",
            Description = "Creates a new book and returns the created book.\n\n" +
            "param book - the created book")]
        public async Task<IActionResult> CreateBook(Book book)
        {
            var createdBook = await _bookService.CreateBook(book);
            return Ok(createdBook);
        }

        [HttpPatch]
        [Route("{ISBN}")]
        [SwaggerOperation(Summary = "Update a book",
            Description = "Updates the details of a book based on the passed ISBN.\n\n" +
            "param book - the updated book")]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }

        [HttpDelete]
        [Route("{ISBN}")]
        [SwaggerOperation(Summary = "Delete a book",
            Description = "Deletes a book based on the passed ISBN.\n\n" +
            "param ISBN - International Standard Book Number")]
        public async Task<IActionResult> DeleteBook(string ISBN)
        {
            var deletedSuccessfully = await _bookService.DeleteBook(ISBN);
            return Ok(deletedSuccessfully);
        }

        [HttpGet]
        [Route("Popular")]
        [SwaggerOperation(Summary = "Get most popular books among students",
            Description = "Returns a list of the most popular books among students.")]
        public async Task<IActionResult> GetMostPopularBooksAmongStudents()
        {
            var books = await _bookService.GetMostPopularBooksAmongStudents();
            return Ok(books);
        }
    }
}
