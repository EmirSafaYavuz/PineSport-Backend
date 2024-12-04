using Business.Authentication.Model;
using Core.Utilities.Results;

namespace Business.Authentication
{
    public class AgentAuthenticationProvider : IAuthenticationProvider
    {
        public Task<LoginUserResult> Login(LoginUserCommand command)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<PineToken>> Verify(VerifyOtpCommand command)
        {
            throw new NotImplementedException();
        }
    }
}