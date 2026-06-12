using LuxeStore.Data;
using LuxeStore.Models;
using LuxeStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LuxeStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= PLACE ORDER =================
        public async Task<Order> PlaceOrderAsync(string userId, CheckoutViewModel model)
        {
            // (skip — already working in your user side)
            throw new NotImplementedException();
        }

        // ================= USER ORDERS =================
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        // ================= GET BY ID 🔥 =================
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // ================= GET ALL 🔥 =================
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User) // 👈 IMPORTANT
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        // ================= UPDATE STATUS =================
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null) return;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}