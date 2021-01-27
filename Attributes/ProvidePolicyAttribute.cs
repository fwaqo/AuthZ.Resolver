using System;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ProvidePolicyAttribute : Attribute
    {
        public string Category { get; set; }
        public string PolicyName { get; }
        public string[] AccessRights { get; }

        public ProvidePolicyAttribute(string category, string policyName, params string[] accessRights)
        {
            Category = string.IsNullOrEmpty(category)
                ? throw new ArgumentNullException(nameof(category))
                : category.Replace(nameof(Controller),  string.Empty).ToLower();
            PolicyName = policyName.ToLower() ?? throw new ArgumentNullException(nameof(policyName));
            AccessRights = accessRights ?? throw new ArgumentNullException(nameof(accessRights));
        }

        public string GetPolicyName()
        {
            return string.Join(".", Category, PolicyName);
        }

        public AuthorizationPolicy GetPolicy(AuthorizationPolicyBuilder policyBuilder)
        {
            policyBuilder.RequireAuthenticatedUser();
            foreach (var right in AccessRights)
            {
                policyBuilder.RequireClaim(JwtClaimTypes.Scope, string.Join(":",right, Category));
            }
            return policyBuilder.Build();
        }
    }

}