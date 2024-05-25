using DataAccess.Models;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateAudio(Audio audio)
        {
            var createdAudio = await _digitalItemService.CreateAudio(audio);
            return Ok(createdAudio);
        }

        [HttpPost]
        [Route("image")]
        public async Task<IActionResult> CreateImage(Image image)
        {
            var createdImage = await _digitalItemService.CreateImage(image);
            return Ok(createdImage);
        }

        [HttpPost]
        [Route("text")]
        public async Task<IActionResult> CreateText(Text text)
        {
            var createdText = await _digitalItemService.CreateText(text);
            return Ok(createdText);
        }

        [HttpPost]
        [Route("video")]
        public async Task<IActionResult> CreateVideo(Video video)
        {
            var createdVideo = await _digitalItemService.CreateVideo(video);
            return Ok(createdVideo);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDigitalItem(DigitalItem digitalItem)
        {
            var updatedDigitalItem = await _digitalItemService.UpdateDigitalItem(digitalItem);
            return Ok(updatedDigitalItem);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDigitalItem(int id)
        {
            var deletedSuccessfully = await _digitalItemService.DeleteDigitalItem(id);
            return Ok(deletedSuccessfully);
        }
    }
}
