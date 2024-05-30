using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookInstanceController : ControllerBase
    {
        private readonly IBookInstanceService _bookInstanceService;

        public BookInstanceController(IBookInstanceService bookInstanceService)
        {
            _bookInstanceService = bookInstanceService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get book instance",
            Description = "Returns a book instance based on the passed ID.\n\n" +
            "param id - Identifier of the book instance")]
        public async Task<IActionResult> GetBookInstanceAsync(int id)
        {
            BookInstance? bookInstance = await _bookInstanceService.GetBookInstance(id);
            if (bookInstance == null)
            {
                return NotFound();
            }
            return Ok(bookInstance);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all book instances",
            Description = "Returns a list of all book instances.")]
        public async Task<IActionResult> ListBookInstancesAsync()
        {
            List<BookInstance>? bookInstances = await _bookInstanceService.ListBookInstances();
            if (bookInstances != null && bookInstances.Count == 0)
            {
                return NotFound();
            }
            return Ok(bookInstances);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new book instance",
            Description = "Creates a new book instance and returns the created instance." +
            "param bookInstance - the created book instance")]
        public async Task<IActionResult> CreateBookInstance(BookInstance bookInstance)
        {
            var createdBookInstance = await _bookInstanceService.CreateBookInstance(bookInstance);
            return Ok(createdBookInstance);
        }

        [HttpPatch]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a book instance",
            Description = "Updates the details of a book instance based on the passed ID.\n\n" +
            "param id - param bookInstance - the updated book instance")]
        public async Task<IActionResult> UpdateBookInstance(BookInstance bookInstance)
        {
            var updatedBookInstance = await _bookInstanceService.UpdateBookInstance(bookInstance);
            return Ok(updatedBookInstance);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a book instance",
            Description = "Deletes a book instance based on the passed ID.\n\n" +
            "param id - Identifier of the book instance")]
        public async Task<IActionResult> DeleteBookInstance(int id)
        {
            var deletedSuccessfully = await _bookInstanceService.DeleteBookInstance(id);
            return Ok(deletedSuccessfully);
        }
    }
}
