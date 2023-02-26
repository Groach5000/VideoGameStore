using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace VideoGameStore.Data.Cart
{
    public class ShoppingCart
    {
        AppDbContext _context;

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where( n => n.ShoppingCartId == ShoppingCartId).Include(n => n.VideoGame).ToList());
        }

        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Amount * n.VideoGame.Price).Sum();

        public void AddItemToCart(VideoGame videoGame)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.ShoppingCartId == ShoppingCartId && n.VideoGame.Id == videoGame.Id);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    VideoGame = videoGame,
                    Amount = 1,
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemToCart(VideoGame videoGame)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.ShoppingCartId == ShoppingCartId && n.VideoGame.Id == videoGame.Id);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
                
            }
            _context.SaveChanges();
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider serviceProvider)
        {
            ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = serviceProvider.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task ClearShoppingCartAsync()
        {
            var shoppingCartItems = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();

            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

            await _context.SaveChangesAsync();
        }


    }
}
