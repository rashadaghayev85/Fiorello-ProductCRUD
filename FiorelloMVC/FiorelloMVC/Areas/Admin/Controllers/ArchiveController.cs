using FiorelloMVC.Data;
using FiorelloMVC.Helpers;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArchiveController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDBContext _context;
        public ArchiveController(ICategoryService categoryService,AppDBContext context)
        {
            _categoryService = categoryService;
            _context = context; 
        }
        [HttpGet]
        public async Task<IActionResult>CategoryArchive(int page = 1)
        {
            var archiveCategory = await _categoryService.GetAllArchivePaginateAsync(page, 4);

            var mappedDatas = _categoryService.GetMappedDatas(archiveCategory);
            int totalPage = await GetPageCountAsync(4);

            Paginate<CategoryProductVM> paginateDatas = new(mappedDatas, totalPage, page);

            return View(paginateDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int categoryCount = await _categoryService.GetCountForArchiveAsync();

            return (int)Math.Ceiling((decimal) categoryCount / take);
        }
        //public async Task<IActionResult> CategoryArchive()
        //{
        //    return View(await _categoryService.GetAllArchiveAsync());
        //}

        [HttpPost]
        public async Task<IActionResult> SetFromArchive(int? id)
        {
            if (id is null) return BadRequest();
            var category = await _categoryService.GetByIdAsync((int)id);
            if (category is null) return NotFound();
            category.SoftDeleted = !category.SoftDeleted;
            // category.SoftDeleted = true;

            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}
