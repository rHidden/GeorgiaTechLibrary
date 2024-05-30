using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get library",
            Description = "Returns a library based on the passed name.\n\n" +
            "param name - Name of the library")]
        public async Task<IActionResult> GetLibrary(string name)
        {
            var library = await _libraryService.GetLibrary(name);
            return Ok(library);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all libraries",
            Description = "Returns a list of all libraries.")]
        public async Task<IActionResult> ListLibraries()
        {
            List<Library> libraries = await _libraryService.ListLibraries();
            if (!libraries.Any())
            {
                return NotFound();
            }
            return Ok(libraries);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new library",
            Description = "Creates a new library and returns the created library.\n\n" +
            "param library - The created library")]
        public async Task<IActionResult> CreateLibrary(Library library)
        {
            var createdLibrary = await _libraryService.CreateLibrary(library);
            return Ok(createdLibrary);
        }

        [HttpPatch]
        [Route("{name}")]
        [SwaggerOperation(Summary = "Update a library",
            Description = "Updates the details of a library.\n\n" +
            "param library - The updated library")]
        public async Task<IActionResult> UpdateLibrary(Library library)
        {
            var updatedLibrary = await _libraryService.UpdateLibrary(library);
            return Ok(updatedLibrary);
        }

        [HttpDelete]
        [Route("{name}")]
        [SwaggerOperation(Summary = "Delete a library",
            Description = "Deletes a library based on the passed name.\n\n" +
            "param name - Name of the library")]
        public async Task<IActionResult> DeleteLibrary(string name)
        {
            var deletedSuccessfully = await _libraryService.DeleteLibrary(name);
            return Ok(deletedSuccessfully);
        }
    }
}