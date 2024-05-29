using FiorelloMVC.Data;
using FiorelloMVC.Helpers.Extensions;
using FiorelloMVC.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FiorelloMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SliderController : Controller
    {
       private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<SliderVM> sliders = await _context.Sliders.Select(m => new SliderVM { Id=m.Id,Image = m.Image })
                                                 .ToListAsync();
            return View(sliders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

         foreach (var item in request.Images) 
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Input can accept only image format");
                    return View();
                }
                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                    return View();
                }
            }
           foreach(var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                // return Content(fileName);

                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await item.SaveFileToLocalAsync(path);
                await _context.Sliders.AddAsync(new Models.Slider { Image = fileName });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id==null)return BadRequest();
            var deleteSlider = await _context.Sliders.FindAsync(id);
            if(deleteSlider==null) return NotFound();
            string path = _env.GenerateFilePath("img",deleteSlider.Image);

            path.DeleteFileFromLocal();
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);    
            //}

            _context.Sliders.Remove(deleteSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(new SliderEditVM { Image=slider.Image});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int?id,SliderEditVM request)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            if (request.NewImage is null) return RedirectToAction(nameof(Index));
            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Input can accept only image format");
                request.Image = slider.Image;
                return View(request);
               
            }
            if (!request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200 KB ");
                request.Image = slider.Image;
                return View(request);
            }
            string oldPath = _env.GenerateFilePath("img", slider.Image);
            oldPath.DeleteFileFromLocal();
            string fileName=Guid.NewGuid().ToString()+"-"+request.NewImage.FileName;
            string newPath = _env.GenerateFilePath("img", fileName);
            await request.NewImage.SaveFileToLocalAsync(newPath);
            slider.Image = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
