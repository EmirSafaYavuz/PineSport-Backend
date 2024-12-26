using Core.Entities.Dtos;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IUserService
{
    IDataResult<List<SidebarMenuDto>> GetSidebarMenu();
}