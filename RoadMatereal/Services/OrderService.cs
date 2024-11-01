using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetCurrentOrderAsync(int userId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task AddOrderItemToOrder(OrderItem orderItem);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task DeleteOrderItemAsync(int orderItemId);
    }

    public class OrderService(RoadMaterialContext context) : IOrderService
    {
        private readonly RoadMaterialContext _context = context;

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include related OrderItems
                .ThenInclude(oi => oi.Material) // Include related Material for each OrderItem
                .Where(o => o.ClientID.ToString() == userId) // Assuming ClientID is an int
                .ToListAsync();
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null) return;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetCurrentOrderAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.Date)
                .SingleOrDefaultAsync(o => o.ClientID == userId);
        }


        public async Task AddOrderItemToOrder(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Material)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Material)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }

}
