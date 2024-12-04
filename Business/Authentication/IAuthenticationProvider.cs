using Business.Authentication.Model;
using Core.Utilities.Results;

namespace Business.Authentication
{
    public interface IAuthenticationProvider
    {
        Task<LoginUserResult> Login(LoginUserCommand command);
        Task<IDataResult<PineToken>> Verify(VerifyOtpCommand command);
    }
}