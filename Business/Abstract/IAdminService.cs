using Business.Authentication.Model;
using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Abstract;

public interface IAdminService
{
    IResult RegisterAdmin(AdminRegisterDto adminRegisterDto);
}