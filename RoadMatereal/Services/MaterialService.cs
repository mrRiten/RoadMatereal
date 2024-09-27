using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public interface IMaterialService
    {
        Task<IEnumerable<Material>> GetAllMaterialsAsync();
        Task<Material> GetMaterialByIdAsync(int id);
        Task CreateMaterialAsync(Material material);
        Task UpdateMaterialAsync(Material material);
        Task DeleteMaterialAsync(int id);
    }

    public class MaterialService(RoadMaterialContext context) : IMaterialService
    {
        private readonly RoadMaterialContext _context = context;

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
        {
            return await _context.Materials
                .Include(m => m.Supplier)
                .ToListAsync();
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
    }

}
