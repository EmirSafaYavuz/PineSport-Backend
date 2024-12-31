using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Abstract;

public interface IParentService
{
    IResult RegisterParent(ParentRegisterDto parentRegisterDto);
    IDataResult<ParentDto> GetParentById(int parentId);
    IDataResult<List<ParentDto>> GetParents();
}