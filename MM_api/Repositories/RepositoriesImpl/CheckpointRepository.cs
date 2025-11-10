using Microsoft.EntityFrameworkCore;
using MM_api.Models;

namespace MM_api.Repositories.RepositoriesImpl
{
    public class CheckpointRepository : ICheckpointRepository
    {
        private readonly MarathonManagementV1Context _context;

        public CheckpointRepository(MarathonManagementV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Checkpoint>> GetAllAsync()
        {
            return await _context.Checkpoints
                .Where(c => c.IsDeleted == null || c.IsDeleted == false)
                .Include(c => c.Marathon)
                .OrderBy(c => c.Sequence)
                .ToListAsync();
        }

        public async Task<IEnumerable<Checkpoint>> GetByMarathonIdAsync(int marathonId)
        {
            return await _context.Checkpoints
                .Where(c => c.MarathonId == marathonId && (c.IsDeleted == null || c.IsDeleted == false))
                .OrderBy(c => c.Sequence)
                .ToListAsync();
        }

        public async Task<Checkpoint?> GetByIdAsync(int id)
        {
            return await _context.Checkpoints
                .Include(c => c.Marathon)
                .FirstOrDefaultAsync(c => c.CheckpointId == id && (c.IsDeleted == null || c.IsDeleted == false));
        }

        public async Task AddAsync(Checkpoint checkpoint)
        {
            await _context.Checkpoints.AddAsync(checkpoint);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Checkpoint checkpoint)
        {
            _context.Checkpoints.Update(checkpoint);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var checkpoint = await _context.Checkpoints.FindAsync(id);
            if (checkpoint != null)
            {
                checkpoint.IsDeleted = true;
                checkpoint.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
