using LuxeStore.Models.ViewModels;
using LuxeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LuxeStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string? category, string? search)
        {
            var products = await _productService.GetAllAsync(category, search);
            var categories = await _productService.GetCategoriesAsync();
            var vm = new ProductViewModel
            {
                Products = products,
                Categories = categories,
                SelectedCategory = category,
                SearchQuery = search
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}