using AutoMapper;
using MM_api.DTOs;
using MM_api.Models;
using MM_api.Repositories;

namespace MM_api.Services.ServicesImpl
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _repository;
        private readonly IMapper _mapper;

        public RegistrationService(IRegistrationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadRegistrationDTO>> GetAllAsync()
        {
            var registrations = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReadRegistrationDTO>>(registrations);
        }

        public async Task<ReadRegistrationDTO?> GetByIdAsync(int id)
        {
            var registration = await _repository.GetByIdAsync(id);
            return _mapper.Map<ReadRegistrationDTO>(registration);
        }

        public async Task CreateAsync(CreateRegistrationDTO dto)
        {
            var registration = _mapper.Map<Registration>(dto);
            await _repository.AddAsync(registration);
        }

        public async Task UpdateAsync(int id, UpdateRegistrationDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
    }
}
