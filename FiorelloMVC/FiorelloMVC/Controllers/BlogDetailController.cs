using FiorelloMVC.Models;
using FiorelloMVC.Services;
using FiorelloMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloMVC.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogDetailController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IActionResult> Index(int?id)
        {
            if (id is null) return BadRequest();
            Blog blog = await _blogService.GetByIdAsync((int)id);
            if (blog == null) return NotFound();
            return View(blog);
           
        }
    }
}
