using Css.Api.Core.Utilities.Extensions;
using Css.Api.Core.Utilities.Filters;
using Css.Api.Reporting.Business.Extensions;
using Css.Api.Reporting.Extensions;
using Css.Api.Reporting.Models.DTO.Auth;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Css.Api.Reporting
{
    /// <summary>
    /// Base method for intilaizing the services and middlewares
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMVCConfiguration()
                .AddCors()
                .AddHealthCheck()
                .AddSwaggerConfiguration(Configuration)
                .AddServicesScope(HostingEnvironment, Configuration)
                .AddDBContextConfiguration(Configuration)
                .AddControllers();           

            var authSettings = services.BuildServiceProvider().GetRequiredService<IOptions<AuthSettings>>().Value;

            string schemeName = "Bearer";
            string authorityUrl = authSettings.AuthorityUrl;
            bool isValidateAudience = false;
            string apiScope = "api_reporting";
            string policyName = "ApiScope";

            services.AddAuthentication(schemeName)
            .AddJwtBearer(schemeName, options =>
            {
                options.Authority = authorityUrl;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = isValidateAudience
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", apiScope);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                await next();
            });

            app.ConfigureCustomExceptionMiddleware();
            app.UseMiddleware<SerilogPropertyContext>();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = Configuration["Environment:SwaggerRoutePrefix"];
                c.DocumentTitle = Configuration["Title"] + " " + Configuration["Version"];
                c.SwaggerEndpoint("/swagger/" + Configuration["Version"] + "/swagger.json", Configuration["Title"] + " " + Configuration["Version"]);
            });

            app.UseReportingFramework();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            
        }
    }
}
