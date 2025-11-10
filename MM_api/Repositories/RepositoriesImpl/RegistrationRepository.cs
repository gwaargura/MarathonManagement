using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly MarathonManagementV1Context _context;

        public RegistrationRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
        {
            return await _context.Registrations
                .Where(r => r.IsDeleted == null || r.IsDeleted == false)
                .Include(r => r.User)
                .Include(r => r.Marathon)
                .Include(r => r.Payment)
                .ToListAsync();
        }

        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Marathon)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.RegistrationId == id && (r.IsDeleted == null || r.IsDeleted == false));
        }

        public async Task AddAsync(Registration registration)
        {
            registration.RegisteredAt = DateTime.UtcNow;
            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                registration.IsDeleted = true;
                registration.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
