using Business.Authentication;
using Business.Authentication.Model;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;

namespace Business.Abstract;

public interface IAuthService
{
    IDataResult<AccessToken> Login(LoginDto loginDto);
    IDataResult<UserDto> GetProfile();
    IResult Logout();
}