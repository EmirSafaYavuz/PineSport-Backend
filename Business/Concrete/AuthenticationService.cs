using System.Linq;
using Business.Abstract;
using Business.Authentication;
using Business.Authentication.Model;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Core.CrossCuttingConcerns.Caching;

namespace Business.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICacheManager _cacheManager;

        public AuthenticationService(IUserRepository userRepository, ITokenHelper tokenHelper, ICacheManager cacheManager)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _cacheManager = cacheManager;
        }

        public IDataResult<AccessToken> LoginUser(LoginDto loginDto)
        {
            // Kullanıcıyı email ile bul
            var user = _userRepository.Query().FirstOrDefault(u => u.Email == loginDto.Email && u.Status);

            if (user == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            // Şifre doğrulama
            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordSalt, user.PasswordHash))
            {
                return new ErrorDataResult<AccessToken>(Messages.PasswordError);
            }
            
            var claims = _userRepository.GetClaims(user.UserId);

            var accessToken = _tokenHelper.CreateToken<PineToken>(user);
            accessToken.Claims = claims.Select(x => x.Name).ToList();
            
            // Kullanıcının refresh token'ını güncelle
            user.RefreshToken = accessToken.RefreshToken;
            _userRepository.Update(user);
            _userRepository.SaveChanges();

            // Yetki bilgilerini cache'e ekle
            _cacheManager.Add($"{CacheKeys.UserIdForClaim}={user.UserId}", claims.Select(x => x.Name));

            return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
        }
    }
}