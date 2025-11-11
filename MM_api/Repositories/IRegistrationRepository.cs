using MM_api.Models;

namespace MM_api.Repositories
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(int id);
        Task<Registration?> GetByIdUserAndMarathonAsync(int userId, int marathonId);
        Task AddAsync(Registration registration);
        Task UpdateAsync(Registration registration);
        Task SoftDeleteAsync(int id);


    }
}
