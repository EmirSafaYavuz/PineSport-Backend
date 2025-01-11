using System.Collections.Generic;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface ISessionRepository : IEntityRepository<Session>
{
    List<Session> GetSessionsByStudentId(int studentId);
    List<Session> GetSessionsByBranchId(int branchId);
    List<Session> GetSessionsByTrainerId(int trainerId);
    void AssignStudentToSession(StudentSession studentSession);
}