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

    public List<Session> GetSessionsByBranchId(int branchId)
    {
        using var context = new ProjectDbContext();
        return context.Sessions
            .Where(s => s.BranchId == branchId)
            .ToList();
    }

    public List<Session> GetSessionsByTrainerId(int trainerId)
    {
        using var context = new ProjectDbContext();
        return context.Sessions
            .Where(s => s.Trainers.Any(t => t.Id == trainerId))
            .ToList();
    }

    public void AssignStudentToSession(StudentSession studentSession)
    {
        using var context = new ProjectDbContext();
        context.StudentSession.Add(studentSession);
        context.SaveChanges();
    }
}