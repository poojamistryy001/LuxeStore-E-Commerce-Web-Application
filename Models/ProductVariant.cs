namespace LuxeStore.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int StockQuantity { get; set; }
        public decimal? PriceAdjustment { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}