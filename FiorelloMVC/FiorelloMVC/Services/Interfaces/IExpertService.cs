using FiorelloMVC.Models;
using FiorelloMVC.ViewModels.Blog;

namespace FiorelloMVC.Services.Interfaces
{
    public interface IExpertService
    {
        Task<IEnumerable<Expert>> GetAllAsync();
    }
}
