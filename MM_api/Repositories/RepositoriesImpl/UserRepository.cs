using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class UserRepository : IUserRepository
    {
        private readonly MarathonManagementV1Context _context;

        public UserRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsDeleted == false || u.IsDeleted == null)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && (u.IsDeleted == false || u.IsDeleted == null));
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
                _context.Users.Update(user);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
