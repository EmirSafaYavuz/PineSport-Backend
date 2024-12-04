using Core.Entities.Concrete;

namespace Business.Authentication
{
    public interface IAuthenticationCoordinator
    {
        IAuthenticationProvider SelectProvider(AuthenticationProviderType type);
    }
}