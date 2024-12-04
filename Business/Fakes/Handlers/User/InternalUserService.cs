using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;

namespace Business.Fakes.Handlers.User;

public class InternalUserService : IInternalUserService
{
    private readonly IUserRepository _userRepository;

    public InternalUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IDataResult<Core.Entities.Concrete.User>> RegisterInternalUserAsync(string email, string password, string fullName)
    {
        var isThereAnyUser = await _userRepository.GetAsync(u => u.Email == email);

        if (isThereAnyUser != null)
        {
            return new ErrorDataResult<Core.Entities.Concrete.User>(Messages.NameAlreadyExist);
        }

        HashingHelper.CreatePasswordHash(password, out var passwordSalt, out var passwordHash);
        var user = new Core.Entities.Concrete.User
        {
            Email = email,

            FullName = fullName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Status = true
        };

        _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();
        return new SuccessDataResult<Core.Entities.Concrete.User>(user, Messages.Added);
    }

    public async Task<IDataResult<Core.Entities.Concrete.User>> GetUserByEmailAsync(string userEmail)
    {
        var user = await _userRepository.GetAsync(u => u.Email == userEmail);

        if (user == null)
        {
            return new ErrorDataResult<Core.Entities.Concrete.User>(Messages.UserNotFound);
        }

        return new SuccessDataResult<Core.Entities.Concrete.User>(user);
    }
}