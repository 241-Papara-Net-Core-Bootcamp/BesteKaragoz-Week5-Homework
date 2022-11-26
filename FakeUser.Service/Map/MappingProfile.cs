
using AutoMapper;
using FakeUser.Domain.Entities;
using FakeUser.Infrastructure.Dtos;

namespace FakeUser.Service.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<FakeUserEntity, FakeUserDto>().ReverseMap();
        }
    }
}
