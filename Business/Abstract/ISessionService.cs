using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface ISessionService
{
    IDataResult<List<SessionDto>> GetSessionsByStudentId(int studentId);
    IDataResult<List<SessionDto>> GetSessionsByBranchId(int branchId);
    IDataResult<List<SessionDto>> GetSessionsByTrainerId(int trainerId);
    IDataResult<List<SessionDto>> GetSessions();
    IDataResult<SessionDto> GetSessionById(int sessionId);
    IResult CreateSession(SessionCreateDto sessionCreateDto);
    IResult UpdateSession(SessionUpdateDto sessionUpdateDto);
    IResult DeleteSession(int sessionId);
    IResult AssignStudentToSession(AssignStudentDto assignStudentDto);
    IResult AssignTrainerToSession(AssignCoachDto assignCoachDto);
}