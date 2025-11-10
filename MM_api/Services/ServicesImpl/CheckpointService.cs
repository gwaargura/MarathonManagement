using AutoMapper;
using MM_api.DTOs;
using MM_api.Models;
using MM_api.Repositories;

namespace MM_api.Services.ServicesImpl
{
    public class CheckpointService : ICheckpointService
    {
        private readonly ICheckpointRepository _repository;
        private readonly IMapper _mapper;

        public CheckpointService(ICheckpointRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadCheckpointDTO>> GetAllAsync()
        {
            var checkpoints = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReadCheckpointDTO>>(checkpoints);
        }

        public async Task<IEnumerable<ReadCheckpointDTO>> GetByMarathonIdAsync(int marathonId)
        {
            var checkpoints = await _repository.GetByMarathonIdAsync(marathonId);
            return _mapper.Map<IEnumerable<ReadCheckpointDTO>>(checkpoints);
        }

        public async Task<ReadCheckpointDTO?> GetByIdAsync(int id)
        {
            var checkpoint = await _repository.GetByIdAsync(id);
            return checkpoint == null ? null : _mapper.Map<ReadCheckpointDTO>(checkpoint);
        }

        public async Task<ReadCheckpointDTO> CreateAsync(CreateCheckpointDTO dto)
        {
            var checkpoint = _mapper.Map<Checkpoint>(dto);
            await _repository.AddAsync(checkpoint);
            return _mapper.Map<ReadCheckpointDTO>(checkpoint);
        }

        public async Task<ReadCheckpointDTO?> UpdateAsync(int id, UpdateCheckpointDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return _mapper.Map<ReadCheckpointDTO>(existing);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.SoftDeleteAsync(id);
            return true;
        }
    }
}
