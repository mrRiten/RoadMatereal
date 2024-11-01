using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoadMatereal.Models;
using RoadMatereal.Services;
using RoadMatereal.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

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