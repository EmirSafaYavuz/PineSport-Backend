using AutoMapper;
using Business.Abstract;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

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

    [ValidationAspect(typeof(SchoolValidator))]
    public IResult RegisterSchool(SchoolRegisterDto schoolRegisterDto)
    {
        var result = BusinessRules.Run(
            CheckIfEmailExists(schoolRegisterDto.Email),
            CheckIfSchoolNameExists(schoolRegisterDto.SchoolName)
        );

        if (result != null)
        {
            return result;
        }

        var passwordHashResult = CreatePasswordHash(schoolRegisterDto.Password);
        if (!passwordHashResult.Success)
        {
            return passwordHashResult;
        }

        var school = _mapper.Map<School>(schoolRegisterDto);
        school.PasswordHash = passwordHashResult.Data.PasswordHash;
        school.PasswordSalt = passwordHashResult.Data.PasswordSalt;
        school.Status = true;

        _schoolRepository.Add(school);

        return new SuccessResult("School successfully registered.");
    }

    public IDataResult<List<SchoolDto>> GetSchools()
    {
        var schools = _schoolRepository.GetList();
        var mappedSchools = _mapper.Map<List<SchoolDto>>(schools);
        return new SuccessDataResult<List<SchoolDto>>(mappedSchools);
    }

    public IDataResult<SchoolDto> GetSchoolById(int schoolId)
    {
        var school = _schoolRepository.Get(s => s.Id == schoolId);
        if (school == null)
        {
            return new ErrorDataResult<SchoolDto>("School not found.");
        }

        var mappedSchool = _mapper.Map<SchoolDto>(school);
        return new SuccessDataResult<SchoolDto>(mappedSchool);
    }

    public IDataResult<SchoolDto> UpdateSchool(SchoolUpdateDto schoolUpdateDto)
    {
        var school = _schoolRepository.Get(s => s.Id == schoolUpdateDto.Id);
        
        if (school == null)
        {
            return new ErrorDataResult<SchoolDto>("School not found.");
        }
        
        var result = BusinessRules.Run(
            CheckIfSchoolNameExists(schoolUpdateDto.SchoolName)
        );
        
        if (result != null)
        {
            return new ErrorDataResult<SchoolDto>(result.Message);
        }
        
        school.SchoolName = schoolUpdateDto.SchoolName;
        school.MobilePhones = schoolUpdateDto.MobilePhone;
        school.Address = schoolUpdateDto.Address;
        school.Notes = schoolUpdateDto.Notes;
        school.Gender = schoolUpdateDto.Gender;
        school.BirthDate = schoolUpdateDto.BirthDate;
        
        _schoolRepository.Update(school);
        
        var updatedSchoolDto = _mapper.Map<SchoolDto>(school);
        return new SuccessDataResult<SchoolDto>(updatedSchoolDto);
    }

    public IResult DeleteSchool(int schoolId)
    {
        var school = _schoolRepository.Get(s => s.Id == schoolId);
        if (school == null)
        {
            return new ErrorResult("School not found.");
        }

        _schoolRepository.Delete(school);

        return new SuccessResult("School deleted successfully.");
    }

    private IResult CheckIfEmailExists(string email)
    {
        var existingUser = _userRepository.Query().FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            return new ErrorResult("A user with this email is already registered.");
        }
        return new SuccessResult();
    }

    private IResult CheckIfSchoolNameExists(string schoolName)
    {
        var existingSchool = _schoolRepository.Get(s => s.SchoolName == schoolName);
        if (existingSchool != null)
        {
            return new ErrorResult("A school with this name already exists.");
        }
        return new SuccessResult();
    }

    private IDataResult<(byte[] PasswordHash, byte[] PasswordSalt)> CreatePasswordHash(string password)
    {
        try
        {
            HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            return new SuccessDataResult<(byte[] PasswordHash, byte[] PasswordSalt)>((passwordHash, passwordSalt));
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<(byte[] PasswordHash, byte[] PasswordSalt)>($"An error occurred while hashing the password: {ex.Message}");
        }
    }
}