using Business.Abstract;
using Business.Authentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User login with email and password
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var result = _authService.Login(loginDto);
            
            return GetResponseOnlyResultData(result);
        }
        
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var result = _authService.GetProfile();
            return GetResponseOnlyResultData(result);
        }
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = _authService.Logout();
            return GetResponseOnlyResultMessage(result);
        }
    }
}