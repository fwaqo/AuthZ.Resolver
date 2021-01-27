using System;
using Authorization.Models;

namespace Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ProvideDefaultWriterPolicyAttribute : ProvidePolicyAttribute
    {

        public ProvideDefaultWriterPolicyAttribute(string categoryName) : base(categoryName, AuthorizationConstants.WriteAccessDisplayName, AuthorizationConstants.WriteAccess)
        {
        }
    }
}