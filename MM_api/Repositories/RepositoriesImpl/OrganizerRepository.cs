using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly MarathonManagementV1Context _context;

        public OrganizerRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organizer>> GetAllAsync()
        {
            return await _context.Organizers
                .Include(o => o.User)
                .Where(o => o.IsDeleted == false || o.IsDeleted == null)
                .ToListAsync();
        }

        public async Task<Organizer?> GetByIdAsync(int id)
        {
            return await _context.Organizers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == id && (o.IsDeleted == false || o.IsDeleted == null));
        }

        public async Task AddAsync(Organizer organizer)
        {
            organizer.CreatedAt = DateTime.UtcNow;
            await _context.Organizers.AddAsync(organizer);
            await _context.SaveChangesAsync();
        }

        public async Task<Organizer?> GetByUserIdAsync(int userId)
        {
            return await _context.Organizers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == userId && (o.IsDeleted == false || o.IsDeleted == null));
        }
        public async Task UpdateAsync(Organizer organizer)
        {
            _context.Organizers.Update(organizer);
            await _context.SaveChangesAsync();
        }

        // Soft delete
        public async Task DeleteAsync(int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null) return;

            organizer.IsDeleted = true;
            organizer.DeletedAt = DateTime.UtcNow;
            _context.Organizers.Update(organizer);
            await _context.SaveChangesAsync();
        }
    }
}
