using MM_api.Models;

namespace MM_api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
