using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;
using RoadMatereal.Services;
using RoadMatereal.ViewModels;
using System.Diagnostics;

namespace RoadMatereal.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
        IMaterialService materialService, ISupplierService supplierService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IMaterialService _materialService = materialService;
        private readonly ISupplierService _supplierService = supplierService;

        public async Task<IActionResult> Index(int? supplierId)
        {
            IEnumerable<Material> materials;
            var supplier = await _supplierService.GetAllSuppliersAsync();

            if (supplierId != null)
            {
                materials = await _materialService.GetBySupplierAsync(supplierId);
            }
            else
            {
                materials = await _materialService.GetAllMaterialsAsync();
            }

            // Создание ViewModel
            var viewModel = new MaterialViewModel
            {
                Materials = materials,
                Suppliers = supplier,
                SelectedSupplierId = supplierId
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int MaterialId)
        {
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
