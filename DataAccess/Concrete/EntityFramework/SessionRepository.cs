using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class SessionRepository : EfEntityRepositoryBase<Session, ProjectDbContext>, ISessionRepository
{
    public List<Session> GetSessionsByStudentId(int studentId)
    {
        using var context = new ProjectDbContext();
        return context.Sessions
            .Where(s => s.StudentSessions.Any(ss => ss.StudentId == studentId))
            .ToList();
    }
}