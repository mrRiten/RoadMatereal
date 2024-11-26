using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync(string? filterName, FilterEnum? filter);
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task CreateSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(int id);
    }

    public class SupplierService(RoadMaterialContext context) : ISupplierService
    {
        private readonly RoadMaterialContext _context = context;

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync(string? filterName, FilterEnum? filter)
        {
            var allSuppliers = await _context.Suppliers
                .Include(s => s.Materials)
                .ToListAsync();


            if ((filter == null && filterName != null) ||
                (filter != null && filterName == null))
            {
                return allSuppliers;
            }

            if (filter == FilterEnum.SupplierName)
            {
                return allSuppliers.Where(s => s.Name == filterName);
            }

            return allSuppliers;
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.Materials)
                .FirstOrDefaultAsync(s => s.IdSupplier == id);
        }

        public async Task CreateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }
    }

}
