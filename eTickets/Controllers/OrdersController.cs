using VideoGameStore.Data.Cart;
using VideoGameStore.Data.Services;
using VideoGameStore.Data.ViewModels;
using VideoGameStore.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace VideoGameStore.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {

        private readonly ShoppingCart _shoppingCart;
        private readonly IMoviesService _moviesService;
        private readonly IOrdersService _ordersService;

        public OrdersController(ShoppingCart shoppingCart, IMoviesService moviesService, IOrdersService ordersService)
        {
            _shoppingCart = shoppingCart;
            _moviesService = moviesService;
            _ordersService = ordersService;
        }


        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var result = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(result);
        }

        public async Task<IActionResult> Index ()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrderByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }


        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemToCart(item);
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder ()
        {
            var items = _shoppingCart.GetShoppingCartItems();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);

            // Remove items from shopping cart
            await _shoppingCart.ClearShoppingCartAsync();

            return View("CompleteOrder");
        }


    }
}
