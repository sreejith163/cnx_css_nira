// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Css.Api.Auth
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddTestUsers(TestUsers.Users);

            builder.AddInMemoryIdentityResources(Config.IdentityResources);
            builder.AddInMemoryApiScopes(Config.ApiScopes);
            builder.AddInMemoryClients(Config.Clients);

            Log.Logger.Information(Configuration["SigningCredential:Certificate"]);
            Log.Logger.Information(Configuration["SigningCredential:Password"]);

            var certificatePath = Path.Combine(AppContext.BaseDirectory, Configuration["SigningCredential:Certificate"]);
            var certificate = new X509Certificate2(certificatePath, Configuration["SigningCredential:Password"]);

            builder.AddSigningCredential(certificate);

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                })
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

            //--------Trial to fix http discovery doc
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            //forwardOptions.KnownNetworks.Add(new IPNetwork(xxxx));


            app.UseForwardedHeaders(forwardOptions);
            //---------

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
    }
}