using System.Collections.Generic;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface IBranchRepository : IEntityRepository<Branch>
{
    public List<Branch> GetBranchesByTrainerId(int trainerId);
}