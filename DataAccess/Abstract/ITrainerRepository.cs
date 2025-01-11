using System.Collections.Generic;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface ITrainerRepository : IEntityRepository<Trainer>
{
    public List<Trainer> GetTrainersByBranchId(int branchId);
}