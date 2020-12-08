using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.Core.Utilities.Extensions
{
    /// <summary>
    ///  Extension for adding the ForwardedHeaderOptions to the service collection
    /// </summary>
    public static class ForwardedHeadersExtension
    {
        /// <summary>
        ///  If a reverse proxy such as Apache or NGNIX is used, then the forwarded headers middleware should be configured 
        ///  and setup before calling the Https redirection middleware. The forwarded headers middleware should set the X-Forwarded-Proto to https. 
        ///  This will enable https offloading at the proxy and a plain http call to the web application with a guarantee that 
        ///  the original call was made over a secure https channel. This is critical especially in non-IIS scenarios such as NGINIX or Apache. 
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddForwardedHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            return services;
        }
    }
}
