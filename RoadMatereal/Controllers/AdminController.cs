using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadMatereal.Models;
using RoadMatereal.Services;
using RoadMatereal.ViewModels;

namespace RoadMatereal.Controllers
{
    [Authorize]
    public class AdminController(IOrderService orderService, IMaterialService materialService,
        ISupplierService supplierService, IStatusService statusService) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IMaterialService _materialService = materialService;
        private readonly ISupplierService _supplierService = supplierService;
        private readonly IStatusService _statusService = statusService;

        // Главная страница панели администратора
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orderCount = (await _orderService.GetAllOrdersAsync()).Count();
            var materialCount = (await _materialService.GetAllMaterialsAsync()).Count();
            var supplierCount = (await _supplierService.GetAllSuppliersAsync()).Count();
            var statusCount = (await _statusService.GetAllStatusesAsync()).Count();

            var dashboardData = new AdminDashboardViewModel
            {
                OrderCount = orderCount,
                MaterialCount = materialCount,
                SupplierCount = supplierCount,
                StatusCount = statusCount
            };

            return View(dashboardData);
        }

        // Список всех заказов
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // Детали заказа
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // Создание заказа (GET)
        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }

        // Создание заказа (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(order);
                return RedirectToAction(nameof(Orders));
            }
            return View(order);
        }

        // Редактирование заказа (GET)
        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // Редактирование заказа (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Orders));
            }
            return View(order);
        }

        // Удаление заказа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Orders));
        }

        // Список всех материалов
        [HttpGet]
        public async Task<IActionResult> Materials()
        {
            var materials = await _materialService.GetAllMaterialsAsync();
            return View(materials);
        }

        // Детали материала
        [HttpGet]
        public async Task<IActionResult> MaterialDetails(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
                return NotFound();

            return View(material);
        }

        // Создание материала (GET)
        [HttpGet]
        public IActionResult CreateMaterial()
        {
            return View();
        }

        // Создание материала (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaterial(Material material)
        {
            if (ModelState.IsValid)
            {
                await _materialService.CreateMaterialAsync(material);
                return RedirectToAction(nameof(Materials));
            }
            return View(material);
        }

        // Редактирование материала (GET)
        [HttpGet]
        public async Task<IActionResult> EditMaterial(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
                return NotFound();

            return View(material);
        }

        // Редактирование материала (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaterial(Material material)
        {
            if (ModelState.IsValid)
            {
                await _materialService.UpdateMaterialAsync(material);
                return RedirectToAction(nameof(Materials));
            }
            return View(material);
        }

        // Удаление материала
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            await _materialService.DeleteMaterialAsync(id);
            return RedirectToAction(nameof(Materials));
        }

        // Список всех поставщиков
        [HttpGet]
        public async Task<IActionResult> Suppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return View(suppliers);
        }

        // Создание поставщика (GET)
        [HttpGet]
        public IActionResult CreateSupplier()
        {
            return View();
        }

        // Создание поставщика (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.CreateSupplierAsync(supplier);
                return RedirectToAction(nameof(Suppliers));
            }
            return View(supplier);
        }

        // Редактирование поставщика (GET)
        [HttpGet]
        public async Task<IActionResult> EditSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // Редактирование поставщика (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.UpdateSupplierAsync(supplier);
                return RedirectToAction(nameof(Suppliers));
            }
            return View(supplier);
        }

        // Удаление поставщика
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            await _supplierService.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Suppliers));
        }

        // Список всех статусов
        [HttpGet]
        public async Task<IActionResult> Statuses()
        {
            var statuses = await _statusService.GetAllStatusesAsync();
            return View(statuses);
        }

        // Создание статуса (GET)
        [HttpGet]
        public IActionResult CreateStatus()
        {
            return View();
        }

        // Создание статуса (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStatus(Status status)
        {
            if (ModelState.IsValid)
            {
                await _statusService.CreateStatusAsync(status);
                return RedirectToAction(nameof(Statuses));
            }
            return View(status);
        }

        // Редактирование статуса (GET)
        [HttpGet]
        public async Task<IActionResult> EditStatus(int id)
        {
            var status = await _statusService.GetStatusByIdAsync(id);
            if (status == null)
                return NotFound();

            return View(status);
        }

        // Редактирование статуса (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(Status status)
        {
            if (ModelState.IsValid)
            {
                await _statusService.UpdateStatusAsync(status);
                return RedirectToAction(nameof(Statuses));
            }
            return View(status);
        }

        // Удаление статуса
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            await _statusService.DeleteStatusAsync(id);
            return RedirectToAction(nameof(Statuses));
        }
    }
}
