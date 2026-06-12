using LuxeStore.Data;
using LuxeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeStore.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LIST =================
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.ActivePage = "Categories";
            ViewBag.SubPage = "ListCategories";

            var query = _context.Categories.AsQueryable();

            // 🔥 Only active categories
            query = query.Where(c => c.IsActive);

            // 🔍 Search
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Name.Contains(search));

            return View(await query.ToListAsync());
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            ViewBag.ActivePage = "Categories";
            ViewBag.SubPage = "AddCategory";
            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile imageFile)
        {
            ViewBag.ActivePage = "Categories";
            ViewBag.SubPage = "AddCategory";

            if (!ModelState.IsValid)
                return View(category);

            // 🔥 Image Upload
            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                category.ImageUrl = "/images/categories/" + fileName;  // 🔥 IMPORTANT
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category added successfully!";
            return RedirectToAction("Index");
        }

        // ================= EDIT GET =================
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.ActivePage = "Categories";
            ViewBag.SubPage = "ListCategories";

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ================= EDIT POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category, IFormFile imageFile)
        {
            ViewBag.ActivePage = "Categories";
            ViewBag.SubPage = "ListCategories";

            var existing = await _context.Categories.FindAsync(category.Id);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(category);

            // update fields
            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.IsActive = category.IsActive;

            // 🔥 Image update
            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existing.ImageUrl = "/images/categories/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        // ================= DELETE (SOFT) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            category.IsActive = false;

            await _context.SaveChangesAsync();

            TempData["success"] = "Category deleted!";
            return RedirectToAction("Index");
        }
    }
}