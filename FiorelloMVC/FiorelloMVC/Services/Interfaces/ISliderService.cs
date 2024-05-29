using FiorelloMVC.Models;

namespace FiorelloMVC.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<SliderInfo> GetSliderInfoAsync();
    }
}
