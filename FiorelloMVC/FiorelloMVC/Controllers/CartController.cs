using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewComponents;
using FiorelloMVC.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace FiorelloMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AppDBContext _context;
        private readonly IProductService _productService;

        public CartController(IHttpContextAccessor accessor,
                              AppDBContext context,
                              IProductService productService)
        {
            _accessor = accessor;
            _context = context;
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var dbProducts = await _productService.GetAllAsync();


            List<BasketProductVM> basketProducts = new();

            foreach (var item in basketDatas)
            {
                var dbProduct = dbProducts.FirstOrDefault(m => m.Id == item.Id);

                basketProducts.Add(new BasketProductVM
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    CategoryName = dbProduct.Category.Name,
                    MainImage = dbProduct.ProductImages.FirstOrDefault(m => m.IsMain).Name,
                    Price = dbProduct.Price,
                    Count = item.Count,
                });
            }

            BasketDetailVM basketDetail = new()
            {
                Products = basketProducts,
                TotalPrice = basketDatas.Sum(m => m.Count * m.Price),
                TotalCount = basketDatas.Count
            };

            return View(basketDetail);
        }

        [HttpPost]
        public IActionResult DeleteProductFromBasket(int? id)
        {
            if (id is null) return BadRequest();

            List<BasketVM> basketDatas = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            basketDatas = basketDatas.Where(m => m.Id != id).ToList();

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));


            int totalCount = basketDatas.Sum(m => m.Count);
            decimal totalPrice = basketDatas.Sum(m => m.Count * m.Price);
            int basketCount = basketDatas.Count;

            return Ok(new { basketCount, totalCount, totalPrice });
        }
        //[HttpPost]
        //public IActionResult Minus(int?id,int?quatity)
        //{
        //    if (id is null) return BadRequest();

        //    List<BasketVM> basketDatas = new();

        //    if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
        //    {
        //        basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
        //    }
        //    BasketVM basket = basketDatas.FirstOrDefault(m => m.Id == id);
        //    basketDatas = basketDatas.Where(m => m.Id != id).ToList();

        //    basket.Id = basket.Id;
        //    basket.Count = basket.Count - 1;
        //    basket.Price = basket.Price;
            
        //    basketDatas.Add(basket);
        //    _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

        //    if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
        //    {
        //        basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
        //    }
        //    int totalCount = basketDatas.Sum(m => m.Count);
        //    decimal totalPrice = basketDatas.Sum(m => m.Count * m.Price);
        //    int basketCount = basketDatas.Count;
        //    int count = (int)(quatity - 1);
        //    return Ok(new { count,basketCount, totalCount, totalPrice });
        //}


        //[HttpPost]
        //public IActionResult Plus(int? id,int?quatity)
        //{
        //    if (id is null) return BadRequest();

        //    List<BasketVM> basketDatas = new();

        //    if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
        //    {
        //        basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
        //    }
        //    BasketVM basket = basketDatas.FirstOrDefault(m => m.Id == id);
        //    basketDatas = basketDatas.Where(m => m.Id != id).ToList();

        //    basket.Id = basket.Id;
        //    basket.Count = basket.Count + 1;
        //    basket.Price = basket.Price;
           
        //    basketDatas.Add(basket);
        //    _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));


        //    int totalCount = basketDatas.Sum(m => m.Count);
        //    decimal totalPrice = basketDatas.Sum(m => m.Count * m.Price);
        //    int basketCount = basketDatas.Count;
        //    int count = (int)(quatity - 1);
        //    return Ok(new { count,basketCount, totalCount, totalPrice });
        //}


        [HttpPost]
        public IActionResult UpdateProductQuantity(int id, int quantity)
        {
            List<BasketVM> basketDatas = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var basketItem = basketDatas.FirstOrDefault(m => m.Id == id);
            if (basketItem != null)
            {
                basketItem.Count = quantity;
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            int totalCount = basketDatas.Sum(m => m.Count);
            decimal totalPrice = basketDatas.Sum(m => m.Count * m.Price);
            int basketCount = basketDatas.Count;

            var itemPrice = basketItem.Price * basketItem.Count;
            var subtotal = basketDatas.Sum(m => m.Count * m.Price);

            return Ok(new { basketCount, totalCount, totalPrice, itemPrice, subtotal });
        }
    }
}
    

