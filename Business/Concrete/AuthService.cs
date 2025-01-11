using System.Linq;
using System.Security;
using System.Security.Claims;
using AutoMapper;
using Business.Abstract;
using Business.Authentication;
using Business.Authentication.Model;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.IoC;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICacheManager _cacheManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenHelper tokenHelper, ICacheManager cacheManager, IRoleRepository roleRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _cacheManager = cacheManager;
            _roleRepository = roleRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public IDataResult<AccessToken> Login(LoginDto loginDto)
        {
            // Kullanıcıyı email ile bul
            var user = _userRepository.Query().FirstOrDefault(u => u.Email == loginDto.Email && u.Status);

            if (user == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            // Şifre doğrulama
            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorDataResult<AccessToken>(Messages.PasswordError);
            }
            
            var claims = _userRepository.GetClaims(user.Id);

            var accessToken = _tokenHelper.CreateToken<PineToken>(user);
            accessToken.Claims = claims.Select(x => x.Name).ToList();
            accessToken.Role = user.Role;
            
            // Kullanıcının refresh token'ını güncelle
            user.RefreshToken = accessToken.RefreshToken;
            _userRepository.Update(user);

            // Yetki bilgilerini cache'e ekle
            _cacheManager.Add($"{CacheKeys.UserIdForClaim}={user.Id}", claims.Select(x => x.Name));

            return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
        }

        public IDataResult<UserDto> GetProfile()
        {
            // Extract the user's email from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                throw new SecurityException("User ID not found in claims.");

            // Retrieve the user using the email
            var user = _userRepository.Query().FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return new ErrorDataResult<UserDto>(Messages.UserNotFound);

            var userDto = _mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto);
        }

        public IResult Logout()
        {
            // Extract the user's email from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                throw new SecurityException("User ID not found in claims.");

            // Retrieve the user using the email
            var user = _userRepository.Query().FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            // Clear the user's refresh token
            user.RefreshToken = null;
            _userRepository.Update(user);

            return new SuccessResult(Messages.SuccessfulLogout);
        }
    }
}