using FiorelloMVC.Models;
using FiorelloMVC.ViewModels.Categories;
using FiorelloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FiorelloMVC.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<CategoryProductVM>> GetAllWithProductAsync();
        Task<Category>GetByIdAsync(int id);
        Task<bool>ExistAsync(string name);
        Task  CreateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistExceptByIdAsync(int id,string name);
        Task<IEnumerable<CategoryArchiveVM>> GetAllArchiveAsync();

        Task<IEnumerable<Category>> GetAllPaginateAsync(int page, int take);
        Task<IEnumerable<Category>> GetAllArchivePaginateAsync(int page, int take);
        IEnumerable<CategoryProductVM> GetMappedDatas(IEnumerable<Category> category);
        Task<int> GetCountAsync();
        Task<int> GetCountForArchiveAsync();
        Task<SelectList> GetAllSelectedAsync();
    }
}
