using LuxeStore.Models;
using LuxeStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxeStore.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ================= ALL ORDERS =================
        public async Task<IActionResult> Index()
        {
            ViewBag.ActivePage = "Orders";
            ViewBag.SubPage = "AllOrders";

            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // ================= PENDING =================
        public async Task<IActionResult> Pending()
        {
            ViewBag.ActivePage = "Orders";
            ViewBag.SubPage = "PendingOrders";

            var orders = (await _orderService.GetAllOrdersAsync())
                .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing)
                .ToList();

            return View("Index", orders);
        }

        // ================= DETAILS 🔥 =================
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ================= UPDATE STATUS 🔥 =================
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);

            TempData["Success"] = "Order updated successfully";

            return RedirectToAction("Details", new { id });
        }

        // ================= COMPLETE =================
        [HttpPost]
        public async Task<IActionResult> Complete(int orderId)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Completed);

            TempData["Success"] = "Order marked as Completed";

            return RedirectToAction("Index");
        }
    }
}