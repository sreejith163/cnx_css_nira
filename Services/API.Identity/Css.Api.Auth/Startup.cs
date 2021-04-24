using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Extensions;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Css.Api.Auth
{
    public class Startup
    {
        /// <summary>
        /// Gets the environment.
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var certificatePath = Path.Combine(AppContext.BaseDirectory, Configuration["SigningCredential:Certificate"]);
            var certificate = new X509Certificate2(certificatePath, Configuration["SigningCredential:Password"], X509KeyStorageFlags.MachineKeySet);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddTestUsers(TestUsers.Users)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseMySql(Configuration.GetConnectionString("Database"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseMySql(Configuration.GetConnectionString("Database"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddSigningCredential(certificate); ;

            services.AddAuthentication()
                .AddOpenIdConnect("adfs", "AD authentication", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = Configuration["ExternalProvider:Provider"];
                    options.ClientId = Configuration["ExternalProvider:ClientId"];
                    options.ClientSecret = Configuration["ExternalProvider:ClientSecret"];
                    options.ResponseType = "id_token";

                    options.CallbackPath = "/signin-adfs";
                    options.SignedOutCallbackPath = "/signout-callback-adfs";
                    options.RemoteSignOutPath = "/signout-adfs";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            services.AddCors(options => options.AddPolicy("AllowAnyOrigin", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(Configuration["IdentityProvider:Authority"]);
                await next();
            });

            InitializeDatabase(app);

            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);

            app.UseStaticFiles();
            app.UseCors("AllowAnyOrigin");

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        /// <summary>
        /// Initializes the database.
        /// </summary>
        /// <param name="app">The application.</param>
        private void InitializeDatabase(IApplicationBuilder app)
        {
            var applicationUrl = Configuration["CssClient:ApplicationUrl"];
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients(applicationUrl))
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}