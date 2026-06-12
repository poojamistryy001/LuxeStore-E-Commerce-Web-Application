using LuxeStore.Models;

namespace LuxeStore.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(string? category = null, string? search = null);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetFeaturedAsync();
    }
}