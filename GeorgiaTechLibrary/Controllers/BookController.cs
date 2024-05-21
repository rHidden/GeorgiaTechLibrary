﻿using DataAccess.Models;
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

        /// <summary>
        /// Retrieves a list of all books.
        /// </summary>
        /// <description>
        /// Description: This endpoint retrieves a list of all books in the database.
        /// </description>
        /// <returns>A list of books.</returns>
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
        public async Task<IActionResult> UpdateBook(Book book)
        {
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(string ISBN)
        {
            var deletedSuccessfully = await _bookService.DeleteBook(ISBN);
            return Ok(deletedSuccessfully);
        }
    }
}
