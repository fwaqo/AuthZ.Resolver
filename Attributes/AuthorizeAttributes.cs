using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Attributes
{
    public class AuthorizeWriterAttribute : AuthorizeAttribute
    {
        public AuthorizeWriterAttribute(string name) : base(TransformName(name))
        {
        }

        private static string TransformName(string name)
        {
            var policyName =  name.Contains(nameof(Controller))
                ? name.Replace(nameof(Controller), string.Empty).ToLower()
                : name;

            return string.Join(".", policyName, AuthorizationConstants.WriterPolicyName);
        }
    }
    
    public class AuthorizeReaderAttribute : AuthorizeAttribute
    {
        public AuthorizeReaderAttribute(string name) : base(TransformName(name))
        {
        }

        private static string TransformName(string name)
        {
            var policyName =  name.Contains(nameof(Controller))
                ? name.Replace(nameof(Controller), string.Empty).ToLower()
                : name;

            return string.Join(".", policyName, AuthorizationConstants.ReaderPolicyName);
        }
    }
}