using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class TrainerRepository : EfEntityRepositoryBase<Trainer, ProjectDbContext>, ITrainerRepository
{
    public List<Trainer> GetTrainersByBranchId(int branchId)
    {
        using var context = new ProjectDbContext();
        return context.Sessions
            .Where(session => session.BranchId == branchId)
            .SelectMany(session => session.Trainers)
            .Distinct()
            .ToList();
    }
}