using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class MarathonRepository : IMarathonRepository
    {
        private readonly MarathonManagementV1Context _context;

        public MarathonRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marathon>> GetAllAsync()
        {
            return await _context.Marathons
                .Where(m => m.IsDeleted == null || m.IsDeleted == false)
                .Include(m => m.Organizer)
                .ToListAsync();
        }

        public async Task<Marathon?> GetByIdAsync(int id)
        {
            return await _context.Marathons
                .Include(m => m.Organizer)
                .FirstOrDefaultAsync(m => m.MarathonId == id && (m.IsDeleted == null || m.IsDeleted == false));
        }

        public async Task AddAsync(Marathon marathon)
        {
            marathon.CreatedAt = DateTime.UtcNow;
            await _context.Marathons.AddAsync(marathon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Marathon marathon)
        {
            _context.Marathons.Update(marathon);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var marathon = await _context.Marathons.FindAsync(id);
            if (marathon != null)
            {
                marathon.IsDeleted = true;
                marathon.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Marathon>> GetOrganizerAsync(int id)
        {
            return await _context.Marathons
                .Where(m => m.IsDeleted == null || m.IsDeleted == false)
                .Where(m => m.OrganizerId == id)
                .ToListAsync();
        }
    }
}
