using Core.Entities.Dtos;
using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.BaseDto;

namespace Business.Abstract;

public interface IUserService
{
    IDataResult<List<SidebarMenuDto>> GetSidebarMenu();
}