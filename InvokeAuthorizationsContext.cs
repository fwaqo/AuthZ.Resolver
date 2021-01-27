using Authorization.Models;
using Waffenschmidt.AuthZ.Resolver.Models;

namespace Authorization
{
    public class InvokeAuthorizationsContext : AuthorizationsInvokedContext
    {
        public PrincipalAuthorizationsRequest Request { get; set; }
    }
}