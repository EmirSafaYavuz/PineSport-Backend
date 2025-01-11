using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface IParentService
{
    IResult RegisterParent(ParentRegisterDto parentRegisterDto);
    IDataResult<ParentDto> GetParentById(int parentId);
    IDataResult<List<ParentDto>> GetParents();
    IDataResult<ParentDto> UpdateParent(ParentUpdateDto parentUpdateDto);
    IResult DeleteParent(int id);
    IDataResult<List<ParentDto>> SearchParentsByName(string name);
}