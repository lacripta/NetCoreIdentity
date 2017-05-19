// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServerWithAspNetIdentity {
    public class Config {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources() {
            var customerProfile = new IdentityResource(
                name: "profile.customer",
                displayName: "Customer profile",
                claimTypes : new [] { "name", "status", "location", "role" ,"prueba" });
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                customerProfile
            };
        }

        public static IEnumerable<ApiResource> GetApiResources() {
            return new List<ApiResource> {
                new ApiResource("api1", "My API") {
                    ApiSecrets = {
                            new Secret("api1secret".Sha256())
                        },
                        UserClaims = new List<string> {
                            JwtClaimTypes.Name,
                            JwtClaimTypes.GivenName,
                            JwtClaimTypes.FamilyName,
                            JwtClaimTypes.Email,
                            JwtClaimTypes.Role,
                            JwtClaimTypes.WebSite,
                            "prueba"
                        }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients() {
            // client credentials client
            return new List<Client> {
                new Client {
                    ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client {
                    ClientId = "ro.client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api1" }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client {
                    ClientId = "mvc",
                        ClientName = "MVC Client",
                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                        RequireConsent = true,

                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },

                        RedirectUris = { "http://elibomdevelopment.com:5002/signin-oidc" },
                        PostLogoutRedirectUris = { "http://elibomdevelopment.com:5002/signout-callback-oidc" },

                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "profile.customer",
                            "api1"
                        },
                        AllowOfflineAccess = true
                }
            };
        }
    }
}