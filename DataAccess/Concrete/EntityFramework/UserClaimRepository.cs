using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class UserClaimRepository : EfEntityRepositoryBase<UserClaim, ProjectDbContext>, IUserClaimRepository
    {

        public async Task<IEnumerable<UserClaim>> BulkInsert(int userId, IEnumerable<UserClaim> userClaims)
        {
            await using var context = new ProjectDbContext();

            var DbClaimList = context.UserClaims.Where(x => x.UserId == userId);

            context.UserClaims.RemoveRange(DbClaimList);
            await context.UserClaims.AddRangeAsync(userClaims);
            return userClaims;
        }

        public async Task<OperationClaim> GetClaimByNameAsync(string claimName)
        {
            // Claim adı boş veya null ise hata
            if (string.IsNullOrWhiteSpace(claimName))
            {
                throw new ArgumentException("Claim adı boş olamaz.", nameof(claimName));
            }
            await using var context = new ProjectDbContext();

            // OperationClaims tablosunda verilen isimle eşleşen claim'i ara
            var claim = await context.OperationClaims.FirstOrDefaultAsync(c => c.Name == claimName);

            return claim;
        }

        public async Task<IEnumerable<SelectionItem>> GetUserClaimSelectedList(int userId)
        {
            await using var context = new ProjectDbContext();

            var list = await (from oc in context.OperationClaims
                join userClaims in context.UserClaims on oc.Id equals userClaims.ClaimId
                where userClaims.UserId == userId
                select new SelectionItem()
                {
                    Id = oc.Id.ToString(),
                    Label = oc.Name
                }).ToListAsync();

            return list;
        }
    }
}