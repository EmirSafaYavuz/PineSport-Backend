using Business.Authentication;
using Business.Authentication.Model;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;

namespace Business.Abstract;

public interface IAuthenticationService
{
    IDataResult<AccessToken> LoginUser(LoginDto loginDto);
}