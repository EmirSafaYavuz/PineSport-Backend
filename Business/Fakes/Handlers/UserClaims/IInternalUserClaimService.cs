using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Fakes.Handlers.UserClaims;

public interface IInternalUserClaimService
{
    Task<IResult> AssignClaimsToUserAsync(int userId, IEnumerable<OperationClaim> operationClaims);
    Task<IDataResult<UserClaim>> CheckUserClaimAsync(int userId, string claimName);
    Task<IResult> AssignClaimToUserAsync(int userId, string claimName);
}