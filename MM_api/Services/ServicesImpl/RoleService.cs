using MM_api.Models;
using MM_api.Repositories;

namespace MM_api.Services.ServicesImpl
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;

        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Role>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Role?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Role role)
        {
            await _repo.AddAsync(role);
        }

        public async Task UpdateAsync(Role role)
        {
            await _repo.UpdateAsync(role);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
