using System.ComponentModel.DataAnnotations;

namespace LuxeStore.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required] public string FullName { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        [Required] public string City { get; set; } = string.Empty;
        [Required] public string PostalCode { get; set; } = string.Empty;
        [Required] public string Country { get; set; } = "India";
        [Required] public string PaymentMethod { get; set; } = "Card";

        // Card details (not stored — sent to Stripe)
        public string? CardNumber { get; set; }
        public string? CardExpiry { get; set; }
        public string? CardCVV { get; set; }
    }
}