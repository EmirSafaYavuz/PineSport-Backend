using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers
{
    public interface IUserContextHelper
    {
        string GetCurrentUserRole();
        int? GetCurrentSchoolId();
        int GetCurrentUserId();
        bool IsInRole(string role);
    }

    public class UserContextHelper : IUserContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type.EndsWith("role"))?.Value;
        }

        public int? GetCurrentSchoolId()
        {
            var schoolIdClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "SchoolId")?.Value;
            return schoolIdClaim != null ? int.Parse(schoolIdClaim) : null;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "UserId")?.Value;
            return userIdClaim != null ? int.Parse(userIdClaim) : 0;
        }

        public bool IsInRole(string role)
        {
            return GetCurrentUserRole()?.Equals(role, StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }
}
