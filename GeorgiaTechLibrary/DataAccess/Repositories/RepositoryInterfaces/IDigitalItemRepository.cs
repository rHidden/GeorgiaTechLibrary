using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IDigitalItemRepository
    {
        Task<DigitalItem?> GetDigitalItem(int id);
        Task<List<DigitalItem?>> ListDigitalItems();
        Task<Audio> CreateAudio(Audio audio);
        Task<Image> CreateImage(Image image);
        Task<Text> CreateText(Text text);
        Task<Video> CreateVideo(Video video);
        Task<DigitalItem> UpdateDigitalItem(DigitalItem digitalItem);
        Task<bool> DeleteDigitalItem(int id);
    }
}
