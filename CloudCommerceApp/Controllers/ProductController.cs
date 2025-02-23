using Application;

using Domain;

using Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudCommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private static List<Product> _products = new List<Product>();
        private static List<string> _categories = new List<string> { "Electronics", "Clothing", "Books", "Home" };


        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> List(string searchText, string category)
        {
            var products = await _productService.GetAllProductsAsync(); // Await the Task

            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                products = products.Where(p => p.Category == category);
            }

            ViewBag.Categories = _categories;
            return View(products.ToList());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = _categories;
            return await Task.FromResult(View("~/Views/Product/Create.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _categories;
            return View(product);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
        public async Task<IActionResult> EditAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = _categories;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var existingProduct = await _productService.GetProductByIdAsync(product.Id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Category = product.Category;

            await _productService.UpdateProductAsync(existingProduct);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product != null)
            {
              await  _productService.DeleteProductAsync(id);
            }
            return RedirectToAction("Index");
        }
    }
}

