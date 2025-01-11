using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface ISessionService
{
    IDataResult<List<SessionDto>> GetSessionsByStudentId(int id);
}