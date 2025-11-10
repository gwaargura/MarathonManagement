using MM_api.DTOs.MM_api.DTOs;

namespace MM_api.Services
{
    public interface IMarathonService
    {
        Task<IEnumerable<ReadMarathonDTO>> GetAllAsync();
        Task<IEnumerable<ReadMarathonDTO>> GetOrganizerAsync(int id);
        Task<ReadMarathonDTO?> GetByIdAsync(int id);
        Task<ReadMarathonDTO> CreateAsync(CreateMarathonDTO dto);
        Task<ReadMarathonDTO?> UpdateAsync(int id, UpdateMarathonDTO dto);
        Task<bool> SoftDeleteAsync(int id);
        Task<ReadMarathonDetailDTO?> GetMarathonDetailsAsync(int id);

    }
}
