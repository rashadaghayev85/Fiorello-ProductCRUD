using FiorelloMVC.Extensions;
using FiorelloMVC.Helpers;
using FiorelloMVC.Migrations;
using FiorelloMVC.Models;
using FiorelloMVC.Services.Interfaces;
using FiorelloMVC.ViewModels.Blog;
using FiorelloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Collections.Immutable;

namespace FiorelloMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment env)
        {
            _productService = productService;
            _categoryService = categoryService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var products = await _productService.GetAllPaginateAsync(page, 4);

            var mappedDatas = _productService.GetMappedDatas(products);
            int totalPage = await GetPageCountAsync(4);

            Paginate<ProductVM> paginateDatas = new(mappedDatas, totalPage, page);

            return View(paginateDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _productService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = await _productService.GetByIdWithAllDatasAsync((int)id);
            if (existProduct is null) return NotFound();

            List<ProductImageVM> images = new();
            foreach (var item in existProduct.ProductImages)
            {
                images.Add(new ProductImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain

                });
            }
            ProductDetailVM response = new()
            {
                Name = existProduct.Name,
                Description = existProduct.Description,
                Category = existProduct.Category.Name,
                Price = existProduct.Price,
                Images = images
            };
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                return View();

            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");

                    return View();
                }
            }
            List<ProductImage> images = new();
            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = _env.GenerateFilePath("img", fileName);
                await item.SaveFileToLocalAsync(path);
                images.Add(new ProductImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;
            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price.Replace(".", ",")),
                ProductImages = images

            };

            await _productService.CreateAsync(product);


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = await _productService.GetByIdWithAllDatasAsync((int)id);
            if (existProduct is null) return NotFound();

            foreach (var item in existProduct.ProductImages)
            {
                string path = _env.GenerateFilePath("img", item.Name);

                path.DeleteFileFromLocal();
            }
            await _productService.DeleteAsync(existProduct);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {


            if (id is null) return BadRequest();

            var existProduct = await _productService.GetByIdWithAllDatasAsync((int)id);

            if (existProduct is null) return NotFound();



            ViewBag.categories = await _categoryService.GetAllSelectedAsync();

            List<ProductImageVM> images = new();

            foreach (var item in existProduct.ProductImages)
            {
                images.Add(new ProductImageVM
                {
                    Id = item.Id,
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            ProductEditVM response = new()
            {
                Name = existProduct.Name,
                Description = existProduct.Description,
               // Price = existProduct.Price.ToString().Replace(",", "."),
                Images = images,
                CategoryId = existProduct.CategoryId,
            };





            return View(response);
            //  return View(new ProductEditVM { Name =existProduct.Name, Description =existProduct.Description,Images=images,Price=prod.Price.ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                var product = await _productService.GetByIdWithAllDatasAsync((int)id);

                List<ProductImageVM> imagesss = new();

                foreach (var item in product.ProductImages)
                {
                    imagesss.Add(new ProductImageVM
                    {
                        Image = item.Name,
                        IsMain = item.IsMain
                    });
                }

                return View(new ProductEditVM { Images = imagesss });

            }

            if (id == null) return BadRequest();
            var products = await _productService.GetByIdWithAllDatasAsync((int)id);
            if (products == null) return NotFound();


            if (request.NewImages is not null)
            {

                List<ProductImage> images = new();
                foreach (var item in request.NewImages)
                {
                    string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                    string path = _env.GenerateFilePath("img", fileName);
                    await item.SaveFileToLocalAsync(path);
                    images.Add(new ProductImage { Name = fileName });
                }

                foreach (var item in request.NewImages)
                {


                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImages", "Input can accept only image format");
                        products.ProductImages = images;
                        return View(request);

                    }
                    if (!item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImages", "Image size must be max 500 KB ");
                        products.ProductImages = images;
                        return View(request);
                    }


                }
                foreach (var item in request.NewImages)
                {
                    string oldPath = _env.GenerateFilePath("img", item.Name);
                    oldPath.DeleteFileFromLocal();
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string newPath = _env.GenerateFilePath("img", fileName);

                    await item.SaveFileToLocalAsync(newPath);


                   


                    products.ProductImages.Add(new ProductImage { Name = fileName });





                    //if (request.Price is not null)
                    //{
                    //    product.Price = request.Price;
                    //}




                    //if (fileName is not null)
                    //{
                    //    products.ProductImages=images;
                    //}

                }

            }

            if (request.Name is not null)
            {
                products.Name = request.Name;
            }
            if (request.Description is not null)
            {
                products.Description = request.Description;
            }
            if(request.CategoryId !=0)
            {
                products.CategoryId = request.CategoryId;   
            }
            if (request.Price is not null)
            {
                products.Price = decimal.Parse(request.Price);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}







            await _productService.EditAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsMain(int? id)
        {
            if (id is null) return BadRequest();
            var productImage = await _productService.GetProductImageByIdAsync((int)id);

            if (productImage is null) return NotFound();

            //var images=await _productService.GetProductByNameAsync(productName);

            //foreach (var item in images.ProductImages)
            //{
            //    item.IsMain = false;
            //}
            var productId = productImage.ProductId;

            var pro = await _productService.GetByIdWithAllDatasAsync(productId);
            foreach (var item in pro.ProductImages)
            {
                item.IsMain = false;
            }
            productImage.IsMain = true;



            await _productService.EditAsync();
            return Redirect($"/admin/product/edit/{productId}");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImageDelete(int? id)
        {
            if (id is null) return BadRequest();
            var productImage = await _productService.GetProductImageByIdAsync((int)id);

            if (productImage is null) return NotFound();

            //var images=await _productService.GetProductByNameAsync(productName);

            //foreach (var item in images.ProductImages)
            //{
            //    item.IsMain = false;
            //}


            string path = _env.GenerateFilePath("img", productImage.Name);

            path.DeleteFileFromLocal();

            var productId = productImage.ProductId;

            var pro = await _productService.GetByIdWithAllDatasAsync(productId);

            await _productService.ImageDeleteAsync(productImage);
            return Redirect($"/admin/product/edit/{productId}");

        }
    }
}
