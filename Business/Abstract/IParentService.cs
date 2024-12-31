using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Abstract;

public interface IParentService
{
    IResult RegisterParent(ParentRegisterDto parentRegisterDto);
}