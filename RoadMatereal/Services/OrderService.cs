using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;
using System.Data;

namespace RoadMatereal.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(string? filterName, FilterEnum? filter);
        Task<Order> GetCurrentOrderAsync(int userId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderItem> GetOrderItemByIdAsync(int itemId);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task AddOrderItemToOrder(OrderItem orderItem);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderAsync(int id);
        Task DeleteOrderItemAsync(int orderItemId);
    }

    public class OrderService(RoadMaterialContext context) : IOrderService
    {
        private readonly RoadMaterialContext _context = context;

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Material) // Include related Material for each OrderItem
                .Where(o => o.ClientID.ToString() == userId) // Assuming ClientID is an int
                .OrderByDescending(o => o.IdOrder)
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
                .Where(o => o.StatusID == 1)
                .OrderByDescending(o => o.Date)
                .SingleOrDefaultAsync(o => o.ClientID == userId);
        }


        public async Task AddOrderItemToOrder(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(string? filterName, FilterEnum? filter)
        {
            var allOrders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Material)
                .ToListAsync();

            if ((filter == null && filterName != null) ||
                (filter != null && filterName == null))
            {
                return allOrders;
            }

            if (filter == FilterEnum.OrderClient)
            {
                return allOrders.Where(o => o.Client.UserName == filterName);
            }
            else if (filter == FilterEnum.OrderStatus)
            {
                return allOrders.Where(o => o.Status.Name == filterName);
            }
            else if (filter == FilterEnum.OrderDate)
            {
                var targetDate = DateTime.Parse(filterName);

                return allOrders.Where(o => o.Date.Year == targetDate.Year &&
                    o.Date.Month == targetDate.Month &&
                    o.Date.Day == targetDate.Day);
            }

            return allOrders;
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

        public async Task<OrderItem> GetOrderItemByIdAsync(int itemId)
        {
            return await _context.OrderItems.FindAsync(itemId);
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }
    }

}
