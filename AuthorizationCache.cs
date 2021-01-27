using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Authorization
{
    public class AuthorizationCache : ApplicationCache<ClaimsIdentity>
    {
        public AuthorizationCache(IMemoryCache cache) : base(cache)
        {
        }

        protected override string GetKey(string key)
        {
            return "Authz" + KeySeparator + key;
        }
        
    }
}