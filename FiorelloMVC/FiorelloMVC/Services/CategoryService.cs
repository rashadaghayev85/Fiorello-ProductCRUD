using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Categories;
using FiorelloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly AppDBContext _context;
        public CategoryService(AppDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Categories.AnyAsync(m => m.Name.Trim() == name.Trim());          
            
        }

        public async Task<bool> ExistExceptByIdAsync(int id,string name)
        {
            return await _context.Categories.AnyAsync(m=>m.Name==name&&m.Id!=id);
        }

        public async Task<IEnumerable<CategoryArchiveVM>> GetAllArchiveAsync()
        {
            IEnumerable<Category> categories = await _context.Categories.IgnoreQueryFilters()
                             .Where(m => m.SoftDeleted)
                             .OrderByDescending(m => m.Id)
                             .ToListAsync();
            return categories.Select(m => new CategoryArchiveVM
            {
                CategoryName = m.Name,
                Id = m.Id,
                CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy"),
                ProductCount=m.Products.Count(),
            });

        }

        public async Task<IEnumerable<Category>> GetAllArchivePaginateAsync(int page, int take)
        {
            return await _context.Categories.Include(m => m.Products).IgnoreQueryFilters()
                             .Where(m => m.SoftDeleted)
                             .OrderByDescending(m => m.Id)
                             .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllPaginateAsync(int page, int take)
        {
            return await _context.Categories.Where(m=>!m.SoftDeleted).Include(m => m.Products)
                                          .Skip((page - 1) * take)
                                          .Take(take)
                                          .ToListAsync();
        }

        public async Task<IEnumerable<CategoryProductVM>> GetAllWithProductAsync()
        {
            IEnumerable<Category> categories = await _context.Categories.Include(m => m.Products)
                                                                       .OrderByDescending(m => m.Id)
                                                                       .ToListAsync();
            return categories.Select(m => new CategoryProductVM
            {
                CategoryName = m.Name,
                Id = m.Id,
                CreatedDate = m.CreatedDate.ToString("dd.MM.yyyy"),
                ProductCount = m.Products.Count
            });
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(m=>m.Id==id);  
        }

        public async Task<int> GetCountForArchiveAsync()
        {
            return await _context.Categories.Where(m=>m.SoftDeleted).CountAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public IEnumerable<CategoryProductVM> GetMappedDatas(IEnumerable<Category> category)
        {
            return category.Select(m => new CategoryProductVM()
            {
                Id = m.Id,
                CreatedDate = m.CreatedDate.ToString("dddd.MM.yyyy"),
                ProductCount=m.Products.Count,
                CategoryName=m.Name
            }); ;
        }

        public async Task<SelectList> GetAllSelectedAsync()
        {
            var categories=await _context.Categories.Where(m=>!m.SoftDeleted).ToListAsync();
            return new SelectList(categories, "Id","Name");
        }
    }

     
       

        
    
}
