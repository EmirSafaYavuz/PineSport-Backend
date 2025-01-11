using System.Collections.Generic;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface ISessionRepository : IEntityRepository<Session>
{
    public List<Session> GetSessionsByStudentId(int studentId);
}