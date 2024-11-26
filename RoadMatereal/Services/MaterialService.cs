using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public interface IMaterialService
    {
        Task<IEnumerable<Material>> GetAllMaterialsAsync(string? filterName, FilterEnum? filter);
        Task<Material> GetMaterialByIdAsync(int id);
        Task CreateMaterialAsync(Material material);

        Task<IEnumerable<Material>> GetBySupplierAsync(int? supplierId);

        Task UpdateMaterialAsync(Material material);
        Task DeleteMaterialAsync(int id);
    }

    public class MaterialService(RoadMaterialContext context) : IMaterialService
    {
        private readonly RoadMaterialContext _context = context;

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync(string? filterName, FilterEnum? filter)
        {
            var allMaterial = await _context.Materials
                .Include(m => m.Supplier)
                .ToListAsync();

            if ((filter == null && filterName != null) ||
                (filter != null && filterName == null))
            {
                return allMaterial;
            }

            if (filter == FilterEnum.MaterialName)
            {
                return allMaterial.Where(m => m.Name == filterName);
            }
            else if (filter == FilterEnum.MaterialSupplier)
            {
                return allMaterial.Where(m => m.Supplier.Name == filterName);
            }

            return allMaterial;
        }

        public async Task<Material> GetMaterialByIdAsync(int id)
        {
            return await _context.Materials
                .Include(m => m.Supplier)
                .FirstOrDefaultAsync(m => m.IdMaterial == id);
        }

        public async Task CreateMaterialAsync(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaterialAsync(Material material)
        {
            _context.Entry(material).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Material>> GetBySupplierAsync(int? supplierId)
        {
            return await _context.Materials
                .Where(m => m.SupplierID == supplierId)
                .ToListAsync();
        }
    }

}
