using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Concrete;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AdminService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public IResult RegisterAdmin(AdminRegisterDto adminRegisterDto)
    {
        var role = _roleRepository.Query().FirstOrDefault(r => r.Name == "Admin");
        if (role == null)
        {
            return new ErrorResult(Messages.RoleNotFound);
        }
            
        if (_userRepository.Query().Any(u => u.Email == adminRegisterDto.Email))
        {
            return new ErrorResult(Messages.UserAlreadyExists);
        }

        // Create a new user
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(adminRegisterDto.Password, out passwordHash, out passwordSalt);

        var user = new Admin
        {
            FullName = adminRegisterDto.FullName,
            Email = adminRegisterDto.Email,
            MobilePhones = adminRegisterDto.MobilePhone,
            BirthDate = adminRegisterDto.BirthDate,
            Gender = adminRegisterDto.Gender,
            Address = adminRegisterDto.Address,
            Notes = adminRegisterDto.Notes,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        _userRepository.Add(user);

        return new SuccessResult(Messages.UserRegistered);
    }
}