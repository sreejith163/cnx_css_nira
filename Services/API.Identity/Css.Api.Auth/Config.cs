// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Css.Api.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api_reporting", "Reporting API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 new Client
                {
                    ClientId = "test.client",
                    ClientName = "Test Client Credential",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F58B".Sha256()) },

                    AllowedScopes = { "api_reporting" }
                },
                  new Client
                {
                    ClientId = "client.cnx1",
                    ClientName = "CNX1",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F258-4058-80CE-1C89C192F2C6".Sha256()) },

                    AllowedScopes = { "api_reporting" }
                },

            };
    }
}