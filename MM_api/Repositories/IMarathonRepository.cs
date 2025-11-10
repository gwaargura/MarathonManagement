using MM_api.Models;

namespace MM_api.Repositories
{
    public interface IMarathonRepository
    {
        Task<IEnumerable<Marathon>> GetAllAsync();
        Task<IEnumerable<Marathon>> GetOrganizerAsync(int id);
        Task<Marathon?> GetByIdAsync(int id);
        Task AddAsync(Marathon marathon);
        Task UpdateAsync(Marathon marathon);
        Task SoftDeleteAsync(int id);

    }
}
