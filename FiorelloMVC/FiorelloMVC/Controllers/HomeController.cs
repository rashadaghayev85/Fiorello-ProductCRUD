
using FiorelloMVC.Services;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels;
using FiorelloMVC.ViewModels.Baskets;
using FiorelloMVC.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics;

namespace FiorelloMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
       private readonly IBlogService _blogService;
        private readonly IExpertService _expertService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _accessor;
        public HomeController(ISliderService sliderService,
                              IBlogService blogService,
                              IExpertService expertService,
                              ICategoryService categoryService,
                              IProductService productService,
                              IHttpContextAccessor accessor)
        {
            _blogService = blogService;
            _sliderService = sliderService;
            _expertService = expertService;
            _categoryService = categoryService;
            _productService = productService;
            _accessor = accessor;
        }
        public async Task<IActionResult> Index()
        {
           // _accessor.HttpContext.Response.Cookies.Append("name", "Reshad");
            HomeVM model = new()
            {
              
              Blogs=await _blogService.GetAllAsync(3),
              Experts=await  _expertService.GetAllAsync(),
              Categories=await _categoryService.GetAllAsync(),
              Products=await _productService.GetAllAsync(),
            };

            //Book book1 = new()
            //{
            //    Id = 1,
            //    Name = "Xosrov"
            //};
            //var serializedData=JsonConvert.SerializeObject(book1);
            //_accessor.HttpContext.Response.Cookies.Append("book",serializedData);
            
            return View(model);
        }
        //public IActionResult GetCookiesData()
        //{
        //    var data = JsonConvert.DeserializeObject<Book>(_accessor.HttpContext.Request.Cookies["book"]);  

        //   return Json(data.Name);
        //}
        [HttpPost]
      public async Task<IActionResult> AddProductToBasket(int?id)
        {
            if (id is null) return BadRequest();
            var product = await _productService.GetByIdAsync((int)id);
            if(product is null) return NotFound();
            List<BasketVM> basketDatas;
            if(_accessor.HttpContext.Request.Cookies["basket"]is not null)
            {
                basketDatas=JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basketDatas = new List<BasketVM>(); 
            }

            var existBasketData=basketDatas.FirstOrDefault(m=>m.Id==id);
            if (existBasketData is not null)
            {
                existBasketData.Count++;
            }
            else
            {

            basketDatas.Add(new BasketVM
            {
                Id = (int)id,
                Price = product.Price,
                Count=1
            });
            }

            _accessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));
            
            int count=basketDatas.Sum(m=>m.Count); 
            decimal totalPrice=basketDatas.Sum(m => m.Price*m.Count);   
                        return Ok(new {count,totalPrice});   
        }
     
    }
}
