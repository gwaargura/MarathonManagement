using MM_api.DTOs;

    public interface IRegistrationService
    {
        Task<IEnumerable<ReadRegistrationDTO>> GetAllAsync();
        Task<ReadRegistrationDTO?> GetByIdAsync(int id);
        Task<ReadRegistrationDTO?> GetByIdUserAndMarathonAsync(int userId, int marathonId);
        Task CreateAsync(CreateRegistrationDTO dto);
    Task UpdateAsync(int id, UpdateRegistrationDTO dto);
        Task SoftDeleteAsync(int id);
}
