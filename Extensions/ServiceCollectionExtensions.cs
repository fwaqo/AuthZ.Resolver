using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Authorization.Attributes;
using Authorization.Models;
using Authorization.Options;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;

namespace Authorization.Extensions
{

    /// <summary>
    /// Class SdkBuilder.
    /// </summary>
    public class AuthorizationBuilder : IAuthorizationBuilder
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="services">The services being configured.</param>
        /// <param name="configuration">The configuration.</param>
        public AuthorizationBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// The services being configured.
        /// </summary>
        public virtual IServiceCollection Services { get; }

       
    }

    public interface IAuthorizationBuilder
    {
        
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        IServiceCollection Services { get; }

    }


    public static class ServiceCollectionExtensions
    {
        public static IAuthorizationBuilder AddAuthorizationServices<T>(this IServiceCollection services, Action<AuthorizationProviderOptions> configureOptions) where T : DelegatingHandler
        {
            var authorizationBuilder = new AuthorizationBuilder(services);
            services.TryAddSingleton<IAuthorizationBuilder>(authorizationBuilder);

            authorizationBuilder.AddAuthorizationBuilder<AuthorizationProvider>();

            services.AddHttpClient("AuthorizationInvoker")
                .AddHttpMessageHandler<T>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadGateway)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                        retryAttempt))));
            
            services.AddAuthorization(ConfigureAuthorization);
            services.AddMemoryCache();
            services.AddTransient<AuthorizationCache>();
            services.Configure<AuthorizationProviderOptions>(configureOptions);
            services.TryAdd(ServiceDescriptor.Transient<IPolicyEvaluator, CustomPolicyEvaluator>());
            services.AddTransient<IPolicyEvaluator, CustomPolicyEvaluator>();
            services.AddSingleton(BuildAccessRights);
            return authorizationBuilder;
        }
        
        public static IAuthorizationBuilder AddAuthorizationServices<T>(this IServiceCollection services)where T : DelegatingHandler
            => services.AddAuthorizationServices<T>(options => {});
        
        
        public static IAuthorizationBuilder AddAuthorizationServices<T>(this IServiceCollection services, IConfiguration configuration)where T : DelegatingHandler
            => services.AddAuthorizationServices<T>(options =>
                configuration.GetSection(AuthorizationConstants.General.Authorization).Bind(options));
        

        private static void ConfigureAuthorization(AuthorizationOptions options)
        {
            var asm = Assembly.GetEntryAssembly();
            var controllers = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList(); //filter controllers

            foreach (var policy in controllers.Select(type => type.GetCustomAttributes()
                .Where(p => typeof(ProvidePolicyAttribute).IsAssignableFrom(p.GetType()))
                .Select(p => (ProvidePolicyAttribute) p).ToList()).SelectMany(policyAttributes => policyAttributes))
            {
                options.AddPolicy(policy.GetPolicyName(), builder => policy.GetPolicy(builder));
            }

            options.AddPolicy("RequireUser",
                builder => builder.RequireClaim(JwtClaimTypes.Subject)
                    .RequireAuthenticatedUser());
        }

        private static Dictionary<string, List<AccessRight>> BuildAccessRights(IServiceProvider serviceProvider)
        {
            var asm = Assembly.GetEntryAssembly();
            var controllers = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList(); //filter controllers

            var rights = new Dictionary<string, List<AccessRight>>();
            foreach (var type in controllers)
            {
                var provideAccessRightsAttribute = TypeDescriptor.GetAttributes(type)
                    .OfType<ProvideAccessRightsAttribute>()
                    ?.FirstOrDefault();
                if (provideAccessRightsAttribute == null ||
                    !provideAccessRightsAttribute.ProvidesAccessRights) continue;


                var accessRightsAttributes = type.GetCustomAttributes()
                    .Where(p => p.GetType() == typeof(DefineAccessRightAttribute))
                    .Select(p => (DefineAccessRightAttribute) p).ToList();

                var providedAccessRights = provideAccessRightsAttribute.GetAccessRights();
                var aRights = (from accessRightsAttribute in accessRightsAttributes
                        let key = accessRightsAttribute.Key
                        let value = accessRightsAttribute.DisplayName
                        select accessRightsAttribute.GetAccessRight(key, value))
                    .Where(a => providedAccessRights.Any(x => x == a.Key)).ToList();

                foreach (var aRight in aRights)
                {
                    if (!rights.ContainsKey(aRight.Category.Key))
                    {
                        rights.Add(aRight.Category.Key, new List<AccessRight>() {aRight});
                    }
                    else
                    {
                        rights.TryGetValue(aRight.Category.Key, out var givenRights);
                        if (givenRights != null)
                        {
                            givenRights.Add(aRight);
                        }
                        else
                        {
                            rights.TryAdd(aRight.Category.Key, new List<AccessRight>() {aRight});
                        }
                    }
                }
            }

            return rights;
        }
    }
}