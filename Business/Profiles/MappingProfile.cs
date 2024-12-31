
using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<School, SchoolDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SchoolName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.SchoolAddress))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.SchoolPhone))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.ManagerEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ManagerPhone, opt => opt.MapFrom(src => src.MobilePhones));

        // SchoolDto -> School
        CreateMap<SchoolDto, School>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SchoolAddress, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.SchoolPhone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ManagerName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ManagerEmail))
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.ManagerPhone));

        CreateMap<Branch, BranchDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.BranchAddress))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.BranchPhone))
            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.SchoolName : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.BranchPhone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.School, opt => opt.Ignore());
        
        CreateMap<BranchRegisterDto, Branch>();
    }
}