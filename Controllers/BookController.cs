using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetBookAsync(string ISBN)
        {
            Book book = await _bookService.GetBook(ISBN);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet]
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
        public async Task<IActionResult> CreateBook(Book book)
        {
            var createdBook = await _bookService.CreateBook(book);
            return Ok(createdBook);
        }

        [HttpPatch]
        public IActionResult UpdateBook(Book book)
        {
            _bookService.UpdateBook(book);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteBook(string ISBN)
        {
            var deletedBook = _bookService.DeleteBook(ISBN);
            return Ok(deletedBook);
        }
    }
}
