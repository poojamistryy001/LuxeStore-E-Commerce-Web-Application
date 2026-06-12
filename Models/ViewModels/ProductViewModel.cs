using LuxeStore.Models;

namespace LuxeStore.Models.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public string? SelectedCategory { get; set; }
        public string? SearchQuery { get; set; }
    }
}