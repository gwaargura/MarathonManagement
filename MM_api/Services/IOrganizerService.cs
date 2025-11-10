using MM_api.Models;

namespace MM_api.Services
{
    public interface IOrganizerService
    {
        Task<IEnumerable<Organizer>> GetAllAsync();
        Task<Organizer> GetByIdAsync(int id);
        Task AddAsync(Organizer organizer);
        Task UpdateAsync(Organizer organizer);
        Task DeleteAsync(int id);
        Task<Organizer> GetByUserId(int userId);
    }
}
