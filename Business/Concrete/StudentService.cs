using Business.Abstract;
using Business.Authentication.Model;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete;

public class StudentService : IStudentService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStudentRepository _studentRepository;

    public StudentService(IRoleRepository roleRepository, IUserRepository userRepository, IStudentRepository studentRepository)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _studentRepository = studentRepository;
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
            RoleId = role.Id,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            BranchId = studentRegisterDto.BranchId,
            ParentId = studentRegisterDto.ParentId,
        };

        _studentRepository.Add(user);

        return new SuccessResult(Messages.UserRegistered);
    }
}