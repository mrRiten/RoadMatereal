using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoadMatereal.Models;
using RoadMatereal.Services;
using RoadMatereal.ViewModels;
using System.Security.Claims;

namespace RoadMatereal.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public UserProfileController(UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        // GET: UserProfile
        // Controllers/UserProfileController.cs
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                Orders = await _orderService.GetOrdersByUserIdAsync(userId) ?? new List<Order>() // Ensure it's not null
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderItemCount(int orderItemId, int newCount)
        {
            var orderItem = await _orderService.GetOrderItemByIdAsync(orderItemId);

            orderItem.Count = newCount;
            orderItem.Price = orderItem.Price * orderItem.Count;

            await _orderService.UpdateOrderItemAsync(orderItem);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            order.StatusID = 2;

            await _orderService.UpdateOrderAsync(order);
            return RedirectToAction("Index");
        }

        // POST: Delete Order
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId); // Implement this method in your IOrderService

            return RedirectToAction("Index");
        }

        // POST: Delete Order Item
        [HttpPost]
        public async Task<IActionResult> DeleteOrderItem(int orderItemId)
        {
            await _orderService.DeleteOrderItemAsync(orderItemId); // Implement this method in your IOrderService

            return RedirectToAction("Index");
        }
    }
}