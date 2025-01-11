using System;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class ProgressRepository : EfEntityRepositoryBase<Progress, ProjectDbContext>, IProgressRepository
{
    
}