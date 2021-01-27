using System;
using Authorization.Models;

namespace Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]

    public class ProvideDefaultReaderPolicyAttribute : ProvidePolicyAttribute
    {
        public ProvideDefaultReaderPolicyAttribute(string categoryName) : base(categoryName, AuthorizationConstants.ReaderPolicyName, AuthorizationConstants.ReadAccess)
        {
        }
    }
}