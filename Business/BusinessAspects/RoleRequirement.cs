using System;
using System.Linq;
using System.Security;
using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business.BusinessAspects
{
    /// <summary>
    /// Bu aspect, metoda erişmek için gereken rolleri parametre olarak alır.
    /// Eğer mevcut kullanıcının rolleri bu parametrelerden en az birine sahip değilse,
    /// SecurityException fırlatır.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RoleRequirement : MethodInterception
    {
        private readonly string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Aspect parametre olarak roller alıyor. Örnek: [RoleRequirement("Admin","Teacher")]
        /// </summary>
        /// <param name="roles">Metodu çalıştırabilecek roller</param>
        public RoleRequirement(params string[] roles)
        {
            // DI nesnelerini çekebilmek için ServiceTool kullanıyoruz.
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _roles = roles;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            // Kullanıcının oturum açmış olup olmadığını kontrol edelim:
            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
            {
                throw new SecurityException("Kullanıcı oturum açmamış.");
            }

            // Kullanıcının rollerini Claims içinden okuyalım.
            // Genellikle claim tipleri "role" veya "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            // şeklinde olabilir. Proje yapınıza göre uyarlayın.
            var userRoles = _httpContextAccessor.HttpContext.User.Claims
                .Where(cl => cl.Type.EndsWith("role", StringComparison.OrdinalIgnoreCase))
                .Select(cl => cl.Value)
                .ToList();

            // İstenen rollerden en az birine sahip mi kontrol edelim.
            bool hasRequiredRole = _roles.Any(reqRole => userRoles.Contains(reqRole, StringComparer.OrdinalIgnoreCase));

            if (!hasRequiredRole)
            {
                throw new SecurityException(Messages.AuthorizationsDenied);
            }
        }
    }
}