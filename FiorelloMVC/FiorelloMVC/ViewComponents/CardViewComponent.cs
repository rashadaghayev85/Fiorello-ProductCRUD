using FiorelloMVC.Data;
using FiorelloMVC.Models;
using FiorelloMVC.Services;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using System;
using System.Collections.Immutable;

namespace FiorelloMVC.ViewComponents
{
    public class CardViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _accessor;
        private readonly AppDBContext _context;
        public CardViewComponent(IProductService productService, IHttpContextAccessor accessor,
                                   AppDBContext context)
        {
            _productService = productService;
            _accessor = accessor;
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> pro = new();

            var basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);


            foreach (var item in basketDatas)
            {
                int id = item.Id;
                var product = _context.Products.Include(m => m.ProductImages).FirstOrDefault();
                pro.Add(product);

            }
            List<CardVM> items = pro.Select(product => new CardVM
            {
                Id = product.Id,
                Image = product.ProductImages.FirstOrDefault(m => m.IsMain).Name,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Count = basketDatas.Find(m => m.Id == product.Id).Count,
                Price = product.Price,
            }).ToList();
            TotalBasketVM model = new()
            {
                Product = items,
                Price = basketDatas.Sum(m => m.Price * m.Count),
            };
            return await Task.FromResult(View(model));

        }
    }
    public class CardVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }

        public decimal Price { get; set; }
        //public List<BasketVM> Products { get; set; }
        //public List<decimal> BasketTotalPrice { get; set; }

        ////public string Image { get; set; }
        //public List<int>Count { get; set; }

    }
    public class TotalBasketVM
    {
        public List<CardVM> Product { get; set; }
        public decimal Price { get; set; }
    }
}
