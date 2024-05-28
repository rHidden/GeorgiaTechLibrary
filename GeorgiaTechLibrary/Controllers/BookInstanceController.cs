using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateBookInstance(BookInstance bookInstance)
        {
            var createdBookInstance = await _bookInstanceService.CreateBookInstance(bookInstance);
            return Ok(createdBookInstance);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateBookInstance(BookInstance bookInstance)
        {
            var updatedBookInstance = await _bookInstanceService.UpdateBookInstance(bookInstance);
            return Ok(updatedBookInstance);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBookInstance(int id)
        {
            var deletedSuccessfully = await _bookInstanceService.DeleteBookInstance(id);
            return Ok(deletedSuccessfully);
        }
    }
}
