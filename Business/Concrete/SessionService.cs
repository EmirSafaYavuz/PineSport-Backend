using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Concrete;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public SessionService(ISessionRepository sessionRepository, IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    public IDataResult<List<SessionDto>> GetSessionsByStudentId(int studentId)
    {
        var sessions = _sessionRepository.GetSessionsByStudentId(studentId);
        var mappedSessions = _mapper.Map<List<SessionDto>>(sessions);
        return new SuccessDataResult<List<SessionDto>>(mappedSessions, "Sessions retrieved successfully.");
    }

    public IDataResult<List<SessionDto>> GetSessionsByBranchId(int branchId)
    {
        var sessions = _sessionRepository.GetSessionsByBranchId(branchId);
        var mappedSessions = _mapper.Map<List<SessionDto>>(sessions);
        return new SuccessDataResult<List<SessionDto>>(mappedSessions, "Sessions retrieved successfully.");
    }

    public IDataResult<List<SessionDto>> GetSessionsByTrainerId(int trainerId)
    {
        var sessions = _sessionRepository.GetSessionsByTrainerId(trainerId);
        var mappedSessions = _mapper.Map<List<SessionDto>>(sessions);
        return new SuccessDataResult<List<SessionDto>>(mappedSessions, "Sessions retrieved successfully.");
    }

    public IDataResult<List<SessionDto>> GetSessions()
    {
        var sessions = _sessionRepository.GetList();
        var mappedSessions = _mapper.Map<List<SessionDto>>(sessions);
        return new SuccessDataResult<List<SessionDto>>(mappedSessions, "All sessions retrieved successfully.");
    }

    public IDataResult<SessionDto> GetSessionById(int sessionId)
    {
        var session = _sessionRepository.Get(s => s.Id == sessionId);
        if (session == null)
            return new ErrorDataResult<SessionDto>("Session not found.");

        var mappedSession = _mapper.Map<SessionDto>(session);
        return new SuccessDataResult<SessionDto>(mappedSession, "Session retrieved successfully.");
    }

    public IResult CreateSession(SessionCreateDto sessionCreateDto)
    {
        var session = _mapper.Map<Session>(sessionCreateDto);
        _sessionRepository.Add(session);
        return new SuccessResult("Session created successfully.");
    }

    public IResult UpdateSession(SessionUpdateDto sessionUpdateDto)
    {
        var session = _sessionRepository.Get(s => s.Id == sessionUpdateDto.Id);
        if (session == null)
            return new ErrorResult("Session not found.");

        _mapper.Map(sessionUpdateDto, session); // Update existing session with new values
        _sessionRepository.Update(session);
        return new SuccessResult("Session updated successfully.");
    }

    public IResult DeleteSession(int sessionId)
    {
        var session = _sessionRepository.Get(s => s.Id == sessionId);
        if (session == null)
            return new ErrorResult("Session not found.");

        _sessionRepository.Delete(session);
        return new SuccessResult("Session deleted successfully.");
    }

    public IResult AssignStudentToSession(AssignStudentDto assignStudentDto)
    {
        var studentSession = new StudentSession
        {
            StudentId = assignStudentDto.StudentId,
            SessionId = assignStudentDto.SessionId
        };

        _sessionRepository.AssignStudentToSession(studentSession);
        return new SuccessResult("Student assigned to session successfully.");
    }

    public IResult AssignTrainerToSession(AssignCoachDto assignCoachDto)
    {
        var session = _sessionRepository.Get(s => s.Id == assignCoachDto.SessionId);
        if (session == null)
            return new ErrorResult("Session not found.");

        var trainer = new Trainer { Id = assignCoachDto.TrainerId };
        session.Trainers.Add(trainer); // Add the trainer to the session
        _sessionRepository.Update(session);
        return new SuccessResult("Coach assigned to session successfully.");
    }
}