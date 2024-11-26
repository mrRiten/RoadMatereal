using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;
using RoadMatereal.Services;
using RoadMatereal.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace RoadMatereal.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
        IMaterialService materialService, ISupplierService supplierService, IOrderService orderService,
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IMaterialService _materialService = materialService;
        private readonly ISupplierService _supplierService = supplierService;
        private readonly IOrderService _orderService = orderService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> Index(int? supplierId)
        {
            IEnumerable<Material> materials;
            var suppliers = await _supplierService.GetAllSuppliersAsync(null, null);

            if (supplierId != null)
            {
                materials = await _materialService.GetBySupplierAsync(supplierId);
            }
            else
            {
                materials = await _materialService.GetAllMaterialsAsync(null, null);
            }

            var viewModel = new MaterialViewModel
            {
                Materials = materials,
                Suppliers = suppliers,
                SelectedSupplierId = supplierId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToOrder(int materialId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login or handle unauthenticated access
                return RedirectToAction("Login", "Account");
            }

            // Get the user ID safely
            var userIdString = _userManager.GetUserId(HttpContext.User);

            if (string.IsNullOrEmpty(userIdString))
            {
                // Handle the case where userId is null or empty
                return BadRequest("User is not authenticated.");
            }

            var userId = int.Parse(userIdString); // Now safe to parse

            // Create or get the current order
            var order = await _orderService.GetCurrentOrderAsync(userId);

            if (order == null)
            {
                order = new Order
                {
                    Date = DateTime.Now,
                    StatusID = 1,
                    ClientID = userId,
                };

                await _orderService.CreateOrderAsync(order);
            }

            // Add the material to the order as an OrderItem
            var material = await _materialService.GetMaterialByIdAsync(materialId);

            if (material != null)
            {
                var orderItem = new OrderItem
                {
                    MaterialID = material.IdMaterial,
                    OrderID = order.IdOrder,
                    Count = 1,
                    Price = material.Price
                };

                await _orderService.AddOrderItemToOrder(orderItem);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}