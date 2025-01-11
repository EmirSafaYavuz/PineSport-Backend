
using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
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
        
        CreateMap<SchoolRegisterDto, School>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName))
            .ForMember(dest => dest.SchoolAddress, opt => opt.MapFrom(src => src.SchoolAddress))
            .ForMember(dest => dest.SchoolPhone, opt => opt.MapFrom(src => src.SchoolPhone))
            .ReverseMap();

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
        
        CreateMap<Trainer, TrainerDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhones))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 0 ? "Erkek" : "Kadın")) // Gender mapping, özelleştirilebilir
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
            .ReverseMap()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Erkek" ? 0 : 1));
        
        CreateMap<TrainerRegisterDto, Trainer>()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Şifre için özel bir işlem yapılabilir
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.RecordDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateContactDate, opt => opt.Ignore());
        
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhones))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 0 ? "Male" : "Female")) // Gender mapping
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : string.Empty))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.FullName : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? 0 : 1))
            .ForMember(dest => dest.Branch, opt => opt.Ignore()) // Branch nesnesi için manuel işlem gerekebilir
            .ForMember(dest => dest.Parent, opt => opt.Ignore());
        
        CreateMap<StudentRegisterDto, Student>()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Şifre için özel bir işlem yapılabilir
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.RecordDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateContactDate, opt => opt.Ignore());
        
        CreateMap<Parent, ParentDto>()
            .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhones))
            .ForMember(dest => dest.ChildrenNames, opt => opt.MapFrom(src => src.Children != null 
                ? src.Children.Select(child => child.FullName).ToList() 
                : new List<string>()))
            .ReverseMap()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.Children, opt => opt.Ignore());
        
        CreateMap<ParentRegisterDto, Parent>()
            .ForMember(dest => dest.MobilePhones, opt => opt.MapFrom(src => src.MobilePhone))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Şifre için özel bir işlem yapılabilir
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.RecordDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateContactDate, opt => opt.Ignore());
    }
}