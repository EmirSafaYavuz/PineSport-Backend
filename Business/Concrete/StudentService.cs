using AutoMapper;
using Business.Abstract;
using Business.Authentication.Model;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Concrete;

public class StudentService : IStudentService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IRoleRepository roleRepository, IUserRepository userRepository, IStudentRepository studentRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public IResult RegisterStudent(StudentRegisterDto studentRegisterDto)
    {
        var role = _roleRepository.Query().FirstOrDefault(r => r.Name == "Student");
        if (role == null)
        {
            return new ErrorResult(Messages.RoleNotFound);
        }
            
        if (_userRepository.Query().Any(u => u.Email == studentRegisterDto.Email))
        {
            return new ErrorResult(Messages.UserAlreadyExists);
        }

        // Create a new user
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(studentRegisterDto.Password, out passwordHash, out passwordSalt);

        var user = new Student
        {
            FullName = studentRegisterDto.FullName,
            Email = studentRegisterDto.Email,
            MobilePhones = studentRegisterDto.MobilePhone,
            BirthDate = studentRegisterDto.BirthDate,
            Gender = studentRegisterDto.Gender,
            Address = studentRegisterDto.Address,
            Notes = studentRegisterDto.Notes,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            BranchId = studentRegisterDto.BranchId,
            ParentId = studentRegisterDto.ParentId,
        };

        _studentRepository.Add(user);

        return new SuccessResult(Messages.UserRegistered);
    }

    public IDataResult<StudentDto> GetStudentById(int studentId)
    {
        var student = _studentRepository.Get(s => s.Id == studentId);
        
        if (student == null)
        {
            return new ErrorDataResult<StudentDto>(Messages.UserNotFound);
        }
        
        var studentDto = _mapper.Map<StudentDto>(student);
        return new SuccessDataResult<StudentDto>(studentDto);
    }

    public IDataResult<List<StudentDto>> GetStudents()
    {
        var students = _studentRepository.GetList().ToList();
        
        var studentDtos = _mapper.Map<List<StudentDto>>(students);
        
        return new SuccessDataResult<List<StudentDto>>(studentDtos);
    }
}