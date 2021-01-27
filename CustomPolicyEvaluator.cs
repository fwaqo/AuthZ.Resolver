using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Extensions;
using Authorization.Interfaces;
using Authorization.Models;
using Authorization.Options;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Authorization
{
    public class CustomPolicyEvaluator : PolicyEvaluator
    {
        private readonly AuthorizationCache _authorizationCache;
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IOptions<AuthorizationProviderOptions> _options;

        public CustomPolicyEvaluator(IAuthorizationService authorization, AuthorizationCache authorizationCache,
            IAuthorizationProvider authorizationProvider, IOptions<AuthorizationProviderOptions> options) :
            base(authorization)
        {
            _authorizationCache = authorizationCache;
            _authorizationProvider = authorizationProvider;
            _options = options;
        }

        public override async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context,
            object resource)
        {
            if (authenticationResult.Succeeded)
            {
                var principal = authenticationResult.Principal;
                var key = principal.FindFirstValue(JwtClaimTypes.Subject) ??
                          principal.FindFirstValue(JwtClaimTypes.ClientId);
              
               var claimsIdentity = await _authorizationProvider.InvokeAuthorizationsAsync(principal, default);
               if (claimsIdentity == null) return PolicyAuthorizationResult.Forbid();
               context.User.AddIdentity(claimsIdentity);
            }

            return await base.AuthorizeAsync(policy, authenticationResult, context, resource);
        }
    }
}