using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoGameStore.Data.Services
{
    public class OrdersService : IOrdersService
    {
        AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Order>> GetOrderByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.VideoGame).Include(n => n.User).ToListAsync();

            if (userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress,
                OrderDateTime= DateTime.Now,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach(var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    VideoGameId = item.VideoGame.Id,
                    Price = item.VideoGame.Price,
                    OrderId = order.Id
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
