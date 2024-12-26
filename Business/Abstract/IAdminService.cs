using Business.Authentication.Model;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IAdminService
{
    IResult RegisterAdmin(AdminUserRegisterDto adminUserRegisterDto);
}