using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Css.Api.Auth
{
    public static class Config
    {
        /// <summary>
        /// Gets the identity resources.
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        /// <summary>
        /// Gets the API scopes.
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api.admin", "Admin API"),
                new ApiScope("api.setup", "Setup API"),
                new ApiScope("api.scheduling", "Scheduling API"),
                new ApiScope("api.job", "Job API"),
                new ApiScope("api.reporting", "Reporting API"),
            };

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="applicationUrl">The application URL.</param>
        /// <returns></returns>
        public static IEnumerable<Client> Clients(string applicationUrl) =>
            new Client[]
            {
                 new Client
                {
                    ClientId = "css.job",
                    ClientName = "CSS Web Job",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("088C3423-04AD-4011-95B6-8D034AB3182C".Sha256()) },
                    AllowedScopes = { "api.reporting" }
                },
                  new Client
                {
                    ClientId = "CNX1",
                    ClientName = "CNX1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F258-4058-80CE-1C89C192F2C6".Sha256()) },
                    AllowedScopes = { "api.reporting" }
                },
                new Client
                {
                    ClientId = "css.web.ui",
                    ClientName = "CSS Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api.admin",
                        "api.setup",
                        "api.scheduling",
                        "api.job",
                        "api.reporting",
                    },
                    RedirectUris = { $"{applicationUrl}/callback" },
                    PostLogoutRedirectUris = { $"{applicationUrl}/signout-callback-oidc" },
                    AllowedCorsOrigins = new List<string> { $"{applicationUrl}" },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}