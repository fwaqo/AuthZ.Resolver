using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Waffenschmidt.AuthZ.Resolver.Models
{
    public class AuthorizationsInvokedContext
    {
        public ClaimsIdentity ClaimsIdentity {get;set;}
    }
}
