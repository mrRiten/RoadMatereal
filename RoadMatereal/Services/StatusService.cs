using Microsoft.EntityFrameworkCore;
using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        Task<Status> GetStatusByIdAsync(int id);
        Task CreateStatusAsync(Status status);
        Task UpdateStatusAsync(Status status);
        Task DeleteStatusAsync(int id);
    }

    public class StatusService : IStatusService
    {
        private readonly RoadMaterialContext _context;

        public StatusService(RoadMaterialContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Include(s => s.Orders)
                .ToListAsync();
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            return await _context.Statuses
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.IdStatus == id);
        }

        public async Task CreateStatusAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Status status)
        {
            _context.Entry(status).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status != null)
            {
                _context.Statuses.Remove(status);
                await _context.SaveChangesAsync();
            }
        }
    }

}
