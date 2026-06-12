using LuxeStore.Models;

namespace LuxeStore.Models.ViewModels
{
    public class CartViewModel
    {
        public Cart? Cart { get; set; }
        public decimal SubTotal => Cart?.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity) ?? 0;
        public decimal Shipping => SubTotal >= 5000 ? 0 : 199;
        public decimal Tax => Math.Round(SubTotal * 0.18m, 2);
        public decimal Total => SubTotal + Shipping + Tax;
        public int ItemCount => Cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
    }
}