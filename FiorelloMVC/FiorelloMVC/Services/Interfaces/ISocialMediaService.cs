using FiorelloMVC.Models;

namespace FiorelloMVC.Services.Interfaces
{
    public interface ISocialMediaService
    {
        Task<IEnumerable<SocialMedia>>GetAllAsync();
    }
}
