using LuxeStore.Models;
using LuxeStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LuxeStore.Controllers.Admin
{

    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        private readonly IProductService _productService;

        public AdminProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ================= LIST =================
        public async Task<IActionResult> Index()
        {
            ViewBag.ActivePage = "Products";
            ViewBag.SubPage = "ListProducts";

            var products = await _productService.GetAllAsync();

            // 🔥 optional filter
            products = products.Where(p => p.IsActive).ToList();

            return View(products);
        }

        // ================= CREATE =================
        public async Task<IActionResult> Create()
        {
            ViewBag.ActivePage = "Products";
            ViewBag.SubPage = "AddProduct";

            ViewBag.Categories = (await _productService.GetCategoriesAsync()).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile, List<string> Sizes)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = (await _productService.GetCategoriesAsync()).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                return View(product);
            }

            // 🔥 IMAGE UPLOAD (SAFE)
            if (imageFile != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }

            // 🔥 SIZE VARIANTS
            if (Sizes != null && Sizes.Any())
            {
                product.Variants = Sizes.Select(s => new ProductVariant
                {
                    Size = s
                }).ToList();
            }

            await _productService.CreateAsync(product);

            TempData["Success"] = "Product added successfully";
            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.ActivePage = "Products";
            ViewBag.SubPage = "ListProducts";

            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = (await _productService.GetCategoriesAsync()).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile, List<string> Sizes)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = (await _productService.GetCategoriesAsync()).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                return View(product);
            }

            var existing = await _productService.GetByIdAsync(product.Id);
            if (existing == null) return NotFound();

            // 🔥 IMAGE UPDATE + DELETE OLD
            if (imageFile != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // delete old image
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }
            else
            {
                product.ImageUrl = existing.ImageUrl;
            }

            // 🔥 VARIANTS UPDATE
            if (Sizes != null)
            {
                product.Variants = Sizes.Select(s => new ProductVariant
                {
                    Size = s
                }).ToList();
            }

            await _productService.UpdateAsync(product);

            TempData["Success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            TempData["Success"] = "Product deleted";
            return RedirectToAction("Index");
        }
    }
}