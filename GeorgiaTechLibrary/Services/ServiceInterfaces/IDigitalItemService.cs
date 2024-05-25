using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IDigitalItemService
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
