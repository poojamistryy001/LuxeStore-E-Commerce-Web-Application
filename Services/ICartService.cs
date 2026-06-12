using LuxeStore.Models;

namespace LuxeStore.Services
{
    public interface ICartService
    {
        Task<Cart?> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, string size, int quantity);
        Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveItemAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
    }
}