using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeorgiaTechLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalItemController : ControllerBase
    {
        private readonly IDigitalItemService _digitalItemService;
        public DigitalItemController(IDigitalItemService digitalItemService)
        {
            _digitalItemService = digitalItemService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get digital item",
            Description = "Returns a digital item based on the passed ID.\n\n" +
            "param id - Identifier of the digital item")]
        public async Task<IActionResult> GetDigitalItemAsync(int id)
        {
            DigitalItem? digitalItem = await _digitalItemService.GetDigitalItem(id);
            if (digitalItem == null)
            {
                return NotFound();
            }
            return Ok(digitalItem);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "List all digital items",
            Description = "Returns a list of all digital items.")]
        public async Task<IActionResult> ListDigitalItemsAsync()
        {
            List<DigitalItem?> digitalItems = await _digitalItemService.ListDigitalItems();
            if (!digitalItems.Any())
            {
                return NotFound();
            }
            return Ok(digitalItems);
        }

        [HttpPost]
        [Route("audio")]
        [SwaggerOperation(Summary = "Create a new audio item",
            Description = "Creates a new audio item and returns the created item.\n\n" +
            "param audio - The created audio item")]
        public async Task<IActionResult> CreateAudio(Audio audio)
        {
            var createdAudio = await _digitalItemService.CreateAudio(audio);
            return Ok(createdAudio);
        }

        [HttpPost]
        [Route("image")]
        [SwaggerOperation(Summary = "Create a new image item",
            Description = "Creates a new image item and returns the created item.\n\n" +
            "param image - The created image item")]
        public async Task<IActionResult> CreateImage(Image image)
        {
            var createdImage = await _digitalItemService.CreateImage(image);
            return Ok(createdImage);
        }

        [HttpPost]
        [Route("text")]
        [SwaggerOperation(Summary = "Create a new text item",
            Description = "Creates a new text item and returns the created item.\n\n" +
            "param text - The created text item")]
        public async Task<IActionResult> CreateText(Text text)
        {
            var createdText = await _digitalItemService.CreateText(text);
            return Ok(createdText);
        }

        [HttpPost]
        [Route("video")]
        [SwaggerOperation(Summary = "Create a new video item",
            Description = "Creates a new video item and returns the created item.\n\n" +
            "param video - The created video item")]
        public async Task<IActionResult> CreateVideo(Video video)
        {
            var createdVideo = await _digitalItemService.CreateVideo(video);
            return Ok(createdVideo);
        }

        [HttpPatch]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a digital item",
            Description = "Updates the details of a digital item.\n\n" +
            "param digitalItem - The updated digital item")]
        public async Task<IActionResult> UpdateDigitalItem(DigitalItem digitalItem)
        {
            var updatedDigitalItem = await _digitalItemService.UpdateDigitalItem(digitalItem);
            return Ok(updatedDigitalItem);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a digital item",
            Description = "Deletes a digital item based on the passed ID.\n\n" +
            "param id - Identifier of the digital item")]
        public async Task<IActionResult> DeleteDigitalItem(int id)
        {
            var deletedSuccessfully = await _digitalItemService.DeleteDigitalItem(id);
            return Ok(deletedSuccessfully);
        }
    }
}
