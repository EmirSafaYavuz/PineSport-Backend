using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Concrete;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SchoolService(ISchoolRepository schoolRepository, IMapper mapper, IUserRepository userRepository)
    {
        _schoolRepository = schoolRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public IResult RegisterSchool(SchoolRegisterDto schoolRegisterDto)
    {
        // 1. Email kontrolü: Aynı email ile kayıtlı bir kullanıcı var mı?
        var existingUser = _userRepository.Query().FirstOrDefault(u => u.Email == schoolRegisterDto.Email);
        if (existingUser != null)
        {
            return new ErrorResult("A user with this email is already registered.");
        }

        // 2. Şifreyi hashle
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(schoolRegisterDto.Password, out passwordHash, out passwordSalt);

        // 3. School (User'dan türeyen entity) oluştur
        var school = new School
        {
            FullName = schoolRegisterDto.FullName,
            Email = schoolRegisterDto.Email,
            MobilePhones = schoolRegisterDto.MobilePhone,
            Address = schoolRegisterDto.Address,
            BirthDate = schoolRegisterDto.BirthDate,
            Gender = schoolRegisterDto.Gender,
            Notes = schoolRegisterDto.Notes,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Status = true,
            SchoolName = schoolRegisterDto.SchoolName,
            SchoolAddress = schoolRegisterDto.SchoolAddress,
            SchoolPhone = schoolRegisterDto.SchoolPhone
        };

        // 4. Okulu veritabanına kaydet
        _schoolRepository.Add(school);

        // 5. İşlem başarılı, sonucu döndür
        return new SuccessResult("School successfully registered.");
    }

    //[RoleRequirement("Admin")]
    public IDataResult<List<SchoolDto>> GetSchools()
    {
        var schools = _schoolRepository.GetList();
        var mappedSchools = _mapper.Map<List<SchoolDto>>(schools);
        return new SuccessDataResult<List<SchoolDto>>(mappedSchools);
    }

    public IDataResult<SchoolDto> GetSchoolById(int schoolId)
    {
        var school = _schoolRepository.Get(s => s.Id == schoolId);
        var mappedSchool = _mapper.Map<SchoolDto>(school);
        return new SuccessDataResult<SchoolDto>(mappedSchool);
    }
}