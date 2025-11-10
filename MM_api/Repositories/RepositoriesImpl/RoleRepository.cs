using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MarathonManagementV1Context _context;

        public RoleRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => r.IsDeleted == false || r.IsDeleted == null)
                .ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id && (r.IsDeleted == false || r.IsDeleted == null));
        }

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        // Soft delete
        public async Task DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return;

            role.IsDeleted = true;
            role.DeletedAt = DateTime.UtcNow;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
