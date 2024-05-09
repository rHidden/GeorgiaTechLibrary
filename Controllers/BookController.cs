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
        public async Task<IActionResult> GetBookAsync(int id)
        {
            Book book = await _bookService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet]
        [Route("list")]
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
        [Route("create")]
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
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            return Ok();
        }
    }
}
