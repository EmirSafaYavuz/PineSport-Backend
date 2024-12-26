
using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<School, SchoolDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SchoolName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.SchoolAddress))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.SchoolPhone))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.ManagerEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ManagerPhone, opt => opt.MapFrom(src => src.MobilePhones));

        // SchoolDto -> School
        CreateMap<SchoolDto, School>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SchoolAddress, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.SchoolPhone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ManagerName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ManagerEmail))
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.ManagerPhone));
    }
}