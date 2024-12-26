using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class UserRepository : EfEntityRepositoryBase<User, ProjectDbContext>, IUserRepository
    {

        public List<OperationClaim> GetClaims(int userId)
        {
            using var context = new ProjectDbContext();

            var result = (from user in context.Users
                join userClaim in context.UserClaims on user.UserId equals userClaim.UserId
                join operationClaim in context.OperationClaims on userClaim.ClaimId equals operationClaim.Id
                where user.UserId == userId
                select new
                {
                    operationClaim.Name
                });

            return result.Select(x => new OperationClaim { Name = x.Name }).Distinct().ToList();
        }

        public async Task<User> GetByRefreshToken(string refreshToken)
        {
            await using var context = new ProjectDbContext();

            return await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.Status);
        }
    }
}