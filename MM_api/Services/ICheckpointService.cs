using MM_api.DTOs;

namespace MM_api.Services
{
    public interface ICheckpointService
    {
        Task<IEnumerable<ReadCheckpointDTO>> GetAllAsync();
        Task<IEnumerable<ReadCheckpointDTO>> GetByMarathonIdAsync(int marathonId);
        Task<ReadCheckpointDTO?> GetByIdAsync(int id);
        Task<ReadCheckpointDTO> CreateAsync(CreateCheckpointDTO dto);
        Task<ReadCheckpointDTO?> UpdateAsync(int id, UpdateCheckpointDTO dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
