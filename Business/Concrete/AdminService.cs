using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Dtos;

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

    public IResult RegisterAdmin(AdminUserRegisterDto adminUserRegisterDto)
    {
        var role = _roleRepository.Query().FirstOrDefault(r => r.Name == "Admin");
        if (role == null)
        {
            return new ErrorResult(Messages.RoleNotFound);
        }
            
        if (_userRepository.Query().Any(u => u.Email == adminUserRegisterDto.Email))
        {
            return new ErrorResult(Messages.UserAlreadyExists);
        }

        // Create a new user
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(adminUserRegisterDto.Password, out passwordHash, out passwordSalt);

        var user = new User
        {
            FullName = adminUserRegisterDto.FullName,
            Email = adminUserRegisterDto.Email,
            MobilePhones = adminUserRegisterDto.MobilePhone,
            BirthDate = adminUserRegisterDto.BirthDate,
            Gender = adminUserRegisterDto.Gender,
            Address = adminUserRegisterDto.Address,
            Notes = adminUserRegisterDto.Notes,
            RoleId = role.Id,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        _userRepository.Add(user);

        return new SuccessResult(Messages.UserRegistered);
    }
}