using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;

namespace Business.Authentication
{
    public class PineToken : AccessToken
    {
        public string ExternalUserId { get; set; }
        public AuthenticationProviderType Provider { get; set; }
        public string OnBehalfOf { get; set; }
    }
}