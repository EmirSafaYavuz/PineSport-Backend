using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;

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

    public IDataResult<List<SessionDto>> GetSessionsByStudentId(int id)
    {
        var sessions = _sessionRepository.GetSessionsByStudentId(id);
        var mappedSessions = _mapper.Map<List<SessionDto>>(sessions);
        return new SuccessDataResult<List<SessionDto>>(mappedSessions);
    }
}