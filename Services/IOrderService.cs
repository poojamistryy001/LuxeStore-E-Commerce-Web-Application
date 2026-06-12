using LuxeStore.Models;
using LuxeStore.Models.ViewModels;

namespace LuxeStore.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(string userId, CheckoutViewModel model);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}