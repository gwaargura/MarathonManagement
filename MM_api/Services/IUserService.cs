using MM_api.DTOs;

namespace MM_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ReadUserDTO>> GetAllAsync();
        Task<ReadUserDTO?> GetByIdAsync(int id);
        Task<ReadUserDTO> CreateAsync(CreateUserDTO dto);
        Task<bool> UpdateAsync(int id, UpdateUserDTO dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
