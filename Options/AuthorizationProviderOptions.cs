using Waffenschmidt.AuthZ.Resolver.Models;

namespace Authorization.Options
{
    public class AuthorizationProviderOptions
    {
        public string Address { get; set; }
        public CacheOptions Cache { get; set; } = new CacheOptions();
        public AuthorizationProviderEvents Events { get; set; } = new AuthorizationProviderEvents();
        public string ResolvePath { get; set; } = "/Authorization/Resolve";
    }
}