using MM_api.Models;
using MM_api.Repositories;

namespace MM_api.Services.ServicesImpl
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _repo;

        public OrganizerService(IOrganizerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Organizer>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Organizer> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Organizer organizer) => await _repo.AddAsync(organizer);

        public async Task UpdateAsync(Organizer organizer) => await _repo.UpdateAsync(organizer);

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<Organizer> GetByUserId(int userId) => await _repo.GetByUserIdAsync(userId);
    }
}
