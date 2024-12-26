using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IParentService
{
    IResult RegisterParent(ParentUserRegisterDto parentUserRegisterDto);
}