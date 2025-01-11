using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class BranchRepository : EfEntityRepositoryBase<Branch, ProjectDbContext>, IBranchRepository
{
    public List<Branch> GetBranchesByTrainerId(int trainerId)
    {
        using var context = new ProjectDbContext();
        return context.Sessions
            .Where(session => session.Trainers.Any(trainer => trainer.Id == trainerId))
            .Select(session => session.Branch)
            .Distinct()
            .ToList();
    }
}