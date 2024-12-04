namespace Business.Fakes.Handlers.OperationClaims;

using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

public class InternalOperationClaimService : IInternalOperationClaimService
{
    private readonly IOperationClaimRepository _operationClaimRepository;

    public InternalOperationClaimService(IOperationClaimRepository operationClaimRepository)
    {
        _operationClaimRepository = operationClaimRepository;
    }

    public async Task<IResult> CreateInternalClaimAsync(string claimName)
    {
        // Yetki adının boş veya null olmadığından emin olun
        if (string.IsNullOrWhiteSpace(claimName))
        {
            return new ErrorResult("Yetki adı boş olamaz.");
        }

        // Yetki zaten varsa ekleme
        if (await IsClaimExistsAsync(claimName))
        {
            return new SuccessResult("Yetki zaten mevcut."); // Hata değil, çünkü internal.
        }

        // Yeni yetki oluştur
        var operationClaim = new OperationClaim
        {
            Name = claimName
        };

        _operationClaimRepository.Add(operationClaim);
        await _operationClaimRepository.SaveChangesAsync();

        return new SuccessResult("Internal yetki başarıyla oluşturuldu.");
    }
    
    public async Task<IDataResult<OperationClaim>> GetClaimByNameAsync(string claimName)
    {
        if (string.IsNullOrWhiteSpace(claimName))
        {
            return new ErrorDataResult<OperationClaim>("Claim adı boş olamaz.");
        }

        var claim = await _operationClaimRepository.GetAsync(c => c.Name == claimName);
        if (claim == null)
        {
            return new ErrorDataResult<OperationClaim>("Claim bulunamadı.");
        }

        return new SuccessDataResult<OperationClaim>(claim);
    }

    private async Task<bool> IsClaimExistsAsync(string claimName)
    {
        return await Task.Run(() => _operationClaimRepository.Query().Any(x => x.Name == claimName));
    }
}