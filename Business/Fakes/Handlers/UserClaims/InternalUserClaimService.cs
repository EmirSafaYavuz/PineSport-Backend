using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Business.Fakes.Handlers.UserClaims;

public class InternalUserClaimService : IInternalUserClaimService
{
    private readonly IUserClaimRepository _userClaimsRepository;

    public InternalUserClaimService(IUserClaimRepository userClaimsRepository)
    {
        _userClaimsRepository = userClaimsRepository;
    }

    public async Task<IResult> AssignClaimsToUserAsync(int userId, IEnumerable<OperationClaim> operationClaims)
    {
        foreach (var claim in operationClaims)
        {
            // Eğer kullanıcıya bu claim zaten atanmışsa atlama yap
            if (await DoesClaimExistForUser(userId, claim.Id))
            {
                continue;
            }

            // Yeni UserClaim ekle
            _userClaimsRepository.Add(new UserClaim
            {
                ClaimId = claim.Id,
                UserId = userId
            });
        }

        // Değişiklikleri kaydet
        await _userClaimsRepository.SaveChangesAsync();
        return new SuccessResult(Messages.Added);
    }

    public async Task<IDataResult<UserClaim>> CheckUserClaimAsync(int userId, string claimName)
    {
        // Claim adı boş veya null kontrolü
        if (string.IsNullOrWhiteSpace(claimName))
        {
            return new ErrorDataResult<UserClaim>("Claim adı boş olamaz.");
        }

        // Kullanıcının sahip olduğu claimleri al
        var userClaims = await _userClaimsRepository.GetUserClaimSelectedList(userId);

        // Claim listesinde arama yap
        var isClaimExists = userClaims.Any(c => c.Label == claimName);

        if (!isClaimExists)
        {
            return new ErrorDataResult<UserClaim>("Belirtilen claim bu kullanıcıya atanmış değil.");
        }

        // Claim bulunduysa, başarılı sonuç döndür
        return new SuccessDataResult<UserClaim>(new UserClaim
        {
            UserId = userId,
            // ClaimId burada gerekirse bulunabilir
        }, "Claim bulundu.");
    }
    
    public async Task<IResult> AssignClaimToUserAsync(int userId, string claimName)
    {
        // Claim adı boş veya null kontrolü
        if (string.IsNullOrWhiteSpace(claimName))
        {
            return new ErrorResult("Claim adı boş olamaz.");
        }

        // Claim adına göre claim'i bul
        var claimResult = await _userClaimsRepository.GetClaimByNameAsync(claimName);

        // Claim bulunamadıysa hata döndür
        if (claimResult == null)
        {
            return new ErrorResult("Belirtilen claim bulunamadı.");
        }

        // Eğer kullanıcıya bu claim zaten atanmışsa hata döndür
        if (await DoesClaimExistForUser(userId, claimResult.Id))
        {
            return new ErrorResult("Kullanıcıya bu claim zaten atanmış.");
        }

        // Yeni UserClaim ekle
        _userClaimsRepository.Add(new UserClaim
        {
            ClaimId = claimResult.Id,
            UserId = userId
        });

        // Değişiklikleri kaydet
        await _userClaimsRepository.SaveChangesAsync();
        return new SuccessResult(Messages.Added);
    }
    
    private async Task<bool> DoesClaimExistForUser(int userId, int claimId)
    {
        // Kullanıcıya bu claim atanmış mı kontrol et
        return (await _userClaimsRepository.GetAsync(x =>
            x.UserId == userId && x.ClaimId == claimId)) != null;
    }
}
