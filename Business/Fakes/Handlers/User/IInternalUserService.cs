using Core.Utilities.Results;

namespace Business.Fakes.Handlers.User;

public interface IInternalUserService
{
    Task<IDataResult<Core.Entities.Concrete.User>> RegisterInternalUserAsync(string email, string password, string fullName);
    Task<IDataResult<Core.Entities.Concrete.User>> GetUserByEmailAsync(string userEmail);
}