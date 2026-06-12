using LuxeStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeStore.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            ViewBag.ActivePage = "Customers";
            ViewBag.SubPage = "ListCustomers";

            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    OrderCount = _context.Orders.Count(o => o.UserId == u.Id)
                })
                .ToListAsync();

            return View(users);
        }

        // DETAILS
        public async Task<IActionResult> Details(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var orders = await _context.Orders
                .Where(o => o.UserId == id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            ViewBag.Orders = orders;

            return View(user);
        }

        // DELETE
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}