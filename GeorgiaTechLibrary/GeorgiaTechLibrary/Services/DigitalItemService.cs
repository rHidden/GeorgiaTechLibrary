using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class DigitalItemService : IDigitalItemService
    {
        private readonly IDigitalItemRepository _digitalItemRepository;
        public DigitalItemService(IDigitalItemRepository digitalItemRepository)
        {
            _digitalItemRepository = digitalItemRepository;
        }
        public async Task<DigitalItem?> GetDigitalItem(int id)
        {
            return await _digitalItemRepository.GetDigitalItem(id);
        }
        public async Task<List<DigitalItem?>> ListDigitalItems()
        {
            return await _digitalItemRepository.ListDigitalItems();
        }
        public async Task<Audio> CreateAudio(Audio audio)
        {
            return await _digitalItemRepository.CreateAudio(audio);
        }
        public async Task<Image> CreateImage(Image image)
        {
            return await _digitalItemRepository.CreateImage(image);
        }
        public async Task<Text> CreateText(Text text)
        {
            return await _digitalItemRepository.CreateText(text);
        }
        public async Task<Video> CreateVideo(Video video)
        {
            return await _digitalItemRepository.CreateVideo(video);
        }
        public async Task<DigitalItem> UpdateDigitalItem(DigitalItem digitalItem)
        {
            return await _digitalItemRepository.UpdateDigitalItem(digitalItem);
        }
        public async Task<bool> DeleteDigitalItem(int id)
        {
            return await _digitalItemRepository.DeleteDigitalItem(id);
        }
    }
}
