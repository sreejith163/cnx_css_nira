using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace Css.Api.Core.Utilities.Filters
{
    /// <summary>
    /// Middleware for adding serilog context properties
    /// </summary>
    public class SerilogPropertyContext
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogPropertyContext" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public SerilogPropertyContext(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("Address", context.Connection.RemoteIpAddress.ToString() ?? "unknown"))
            {
                await _next.Invoke(context);
            }
        }
    }
}
