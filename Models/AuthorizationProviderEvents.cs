using System;
using System.Threading.Tasks;
using Authorization;

namespace Waffenschmidt.AuthZ.Resolver.Models
{
    public class AuthorizationProviderEvents
    {
        /// <summary>
        /// A delegate assigned to this property will be invoked when the related method is called.
        /// </summary>
        public Func<AuthorizationsInvokedContext, Task> OnAuthorizationsInvoked { get; set; } = context => Task.CompletedTask;


        /// <summary>
        /// Implements the interface method by invoking the related delegate method.
        /// </summary>
        /// <param name="context">Contains information about the event</param>
        public virtual Task AuthorizationsInvoked(AuthorizationsInvokedContext context) => OnAuthorizationsInvoked(context);

        /// <summary>
        /// A delegate assigned to this property will be invoked when the related method is called.
        /// </summary>
        public Func<InvokeAuthorizationsContext, Task> OnInvokedAuthorizations { get; set; } = context => Task.CompletedTask;

        /// <summary>
        /// Implements the interface method by invoking the related delegate method.
        /// </summary>
        /// <param name="context">Contains information about the event</param>
        public virtual Task InvokeAuthorizations(InvokeAuthorizationsContext context) => OnInvokedAuthorizations(context);

        
    }
}