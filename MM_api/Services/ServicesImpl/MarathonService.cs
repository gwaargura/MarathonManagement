using AutoMapper;
using MM_api.DTOs;
using MM_api.DTOs.MM_api.DTOs;
using MM_api.Models;
using MM_api.Repositories;

namespace MM_api.Services.ServicesImpl
{
    public class MarathonService : IMarathonService
    {
        private readonly IMarathonRepository _repository;
        private readonly ICheckpointService _checkpointService;
        private readonly IMapper _mapper;

        public MarathonService(IMarathonRepository repository, ICheckpointService checkpointService, IMapper mapper)
        {
            _repository = repository;
            _checkpointService = checkpointService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadMarathonDTO>> GetAllAsync()
        {
            var marathons = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReadMarathonDTO>>(marathons);
        }

        public async Task<ReadMarathonDTO?> GetByIdAsync(int id)
        {
            var marathon = await _repository.GetByIdAsync(id);
            return marathon == null ? null : _mapper.Map<ReadMarathonDTO>(marathon);
        }

        public async Task<ReadMarathonDTO> CreateAsync(CreateMarathonDTO dto)
        {
            var marathon = _mapper.Map<Marathon>(dto);
            marathon.Status = "Upcoming";
            await _repository.AddAsync(marathon);
            return _mapper.Map<ReadMarathonDTO>(marathon);
        }

        public async Task<ReadMarathonDTO?> UpdateAsync(int id, UpdateMarathonDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return _mapper.Map<ReadMarathonDTO>(existing);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.SoftDeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ReadMarathonDTO>> GetOrganizerAsync(int id)
        {
            var marathons = await _repository.GetOrganizerAsync(id);
            return _mapper.Map<IEnumerable<ReadMarathonDTO>>(marathons);
        }

        public async Task<ReadMarathonDetailDTO?> GetMarathonDetailsAsync(int id)
        {
            try
            {
                var marathon = await _repository.GetByIdAsync(id);
                if (marathon == null)
                    return null;

                var checkpoints = await _checkpointService.GetByMarathonIdAsync(id);

                return new ReadMarathonDetailDTO
                {
                    MarathonId = marathon.MarathonId,
                    OrganizerName = marathon.Organizer?.OrganizationName ?? "Không xác định",
                    MarathonName = marathon.MarathonName,
                    ThumbnailLink = marathon.ThumbnailLink,
                    Description = marathon.Description,
                    Location = marathon.Location,
                    StartDate = marathon.StartDate,
                    EndDate = marathon.EndDate,
                    RegistrationFee = marathon.RegistrationFee,
                    MaxParticipants = marathon.MaxParticipants,
                    Status = marathon.Status,
                    Checkpoints = checkpoints?.ToList() ?? new List<ReadCheckpointDTO>()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
