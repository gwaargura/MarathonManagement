using AutoMapper;
using MM_api.DTOs;
using MM_api.DTOs.MM_api.DTOs;
using MM_api.Models;

namespace MM_api.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, ReadUserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();

            // Role mappings
            CreateMap<Role, ReadRoleDTO>();
            CreateMap<CreateRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();

            // Organizer mappings
            CreateMap<Organizer, ReadOrganizerDTO>();
            CreateMap<CreateOrganizerDTO, Organizer>();
            CreateMap<UpdateOrganizerDTO, Organizer>();

            CreateMap<Marathon, ReadMarathonDTO>();
            CreateMap<CreateMarathonDTO, Marathon>();
            CreateMap<UpdateMarathonDTO, Marathon>();

            CreateMap<Checkpoint, ReadCheckpointDTO>();
            CreateMap<CreateCheckpointDTO, Checkpoint>();
            CreateMap<UpdateCheckpointDTO, Checkpoint>();

            CreateMap<Registration, ReadRegistrationDTO>();
            CreateMap<CreateRegistrationDTO, Registration>();
            CreateMap<UpdateRegistrationDTO, Registration>();

        }
    }
}
