using FiorelloMVC.Data;
using FiorelloMVC.Helpers.Extensions;
using FiorelloMVC.Models;
using FiorelloMVC.Services;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Blog;
using FiorelloMVC.ViewModels.Categories;
using FiorelloMVC.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(IBlogService blogService,
                                   AppDBContext context,
                                   IWebHostEnvironment env)
        {
            _blogService = blogService;
            _context = context;
            _env = env; 
        }
        public async Task<IActionResult> Index()
        {
            return View(await _blogService.GetAllAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            // return Content(fileName);

            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await request.Image.SaveFileToLocalAsync(path);


            bool existBlog = await _blogService.ExistAsync(request.Title);
            if (existBlog)
            {
                ModelState.AddModelError("Title", "This title already exist");
                return View();
            }
            await _blogService.CreateAsync(new Blog { Title = request.Title,Description=request.Description,Image=fileName});
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete(int? id)
        {
            if (id is null) return BadRequest();
            var deleteBlog = await _context.Blogs.FindAsync(id);
            if (deleteBlog is null) return NotFound();

            string path = _env.GenerateFilePath("img", deleteBlog.Image);

            
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            _context.Blogs.Remove(deleteBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




           

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int?id)
        {
            
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            return View(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            //return View();
            return View(new BlogEditVM {Title=blog.Title,Description=blog.Description, Image = blog.Image });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogEditVM request)
        {
            if (id == null) return BadRequest();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Input can accept only image format");
                request.Image = blog.Image;
                return View(request);

            }
            if (!request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200 KB ");
                request.Image = blog.Image;
                return View(request);
            }
            string oldPath = _env.GenerateFilePath("img", blog.Image);
            oldPath.DeleteFileFromLocal();
            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
            string newPath = _env.GenerateFilePath("img", fileName);
            
            await request.NewImage.SaveFileToLocalAsync(newPath);
           if(request.Title is not null)
            {
                blog.Title = request.Title;
            }
            if (request.Description is not null)
            {
                blog.Description = request.Description ;
            }
            if (fileName is not null)
            {
                blog.Image =fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
