using LuxeStore.Data;
using LuxeStore.Models;
using LuxeStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeStore.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ActivePage = "Dashboard";

            var model = new AdminDashboardViewModel();

            // CARDS
            model.TotalOrders = await _context.Orders.CountAsync();
            model.TotalProducts = await _context.Products.CountAsync();
            model.TotalCustomers = await _context.Users.CountAsync();

            model.TotalRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Completed || o.Status == OrderStatus.Delivered)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            // ORDER STATUS
            model.PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);

            model.ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Processing);

            model.DeliveredOrders = await _context.Orders.CountAsync(o =>
                o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Completed);

            model.CancelledOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Cancelled);

            // RECENT ORDERS
            model.RecentOrders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new RecentOrderRow
                {
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.User != null ? o.User.UserName : "Guest",
                    Total = o.TotalAmount,
                    Status = o.Status.ToString(),
                    Date = o.CreatedAt
                })
                .ToListAsync();

            // TOP PRODUCTS
            model.TopProducts = await _context.OrderItems
                .GroupBy(o => o.Product.Name)
                .Select(g => new TopProductRow
                {
                    Name = g.Key,
                    SoldQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.SoldQuantity)
                .Take(5)
                .ToListAsync();

            return View(model);
        }
    }
}