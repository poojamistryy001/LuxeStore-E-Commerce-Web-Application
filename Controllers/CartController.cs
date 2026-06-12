using LuxeStore.Models.ViewModels;
using LuxeStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LuxeStore.Controllers
{
    [Authorize]
    
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _cartService.GetCartAsync(userId);
            var vm = new CartViewModel { Cart = cart };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, string size, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _cartService.AddToCartAsync(userId, productId, size, quantity);
            TempData["Success"] = "Item added to cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(int cartItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _cartService.UpdateQuantityAsync(userId, cartItemId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _cartService.RemoveItemAsync(userId, cartItemId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Count()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var count = await _cartService.GetCartCountAsync(userId);
            return Json(count);
        }
    }
}