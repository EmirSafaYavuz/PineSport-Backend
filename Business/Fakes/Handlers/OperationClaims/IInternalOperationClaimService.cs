using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Fakes.Handlers.OperationClaims;

public interface IInternalOperationClaimService
{
    Task<IResult> CreateInternalClaimAsync(string claimName);
    Task<IDataResult<OperationClaim>> GetClaimByNameAsync(string claimName);
}