using DataAccess.Models;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;
        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }   

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetLibrary(string name)
        {
            var library = await _libraryService.GetLibrary(name);
            return Ok(library);
        }

        [HttpGet]
        public async Task<IActionResult> ListLibraries()
        {
            List<Library> librarys = await _libraryService.ListLibraries();
            if (!librarys.Any())
            {
                return NotFound();
            }
            return Ok(librarys);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLibrary(Library library)
        {
            var createdLibrary = await _libraryService.CreateLibrary(library);
            return Ok(createdLibrary);
        }

        [HttpPatch]
        public IActionResult UpdateLibrary(Library library)
        {
            _libraryService.UpdateLibrary(library);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteLibrary(string name)
        {
            _libraryService.DeleteLibrary(name);
            return Ok();
        }
    }
}
