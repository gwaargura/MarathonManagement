using MM_api.Models;

namespace MM_api.Repositories
{
    public interface ICheckpointRepository
    {
        Task<IEnumerable<Checkpoint>> GetAllAsync();
        Task<IEnumerable<Checkpoint>> GetByMarathonIdAsync(int marathonId);
        Task<Checkpoint?> GetByIdAsync(int id);
        Task AddAsync(Checkpoint checkpoint);
        Task UpdateAsync(Checkpoint checkpoint);
        Task SoftDeleteAsync(int id);
    }
}
