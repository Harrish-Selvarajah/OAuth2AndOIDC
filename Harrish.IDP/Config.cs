﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Harrish.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", 
                "Your role(s)", 
                new [] { "role" }),
            new IdentityResource("country", "The country your're living in", new List<string>() {  "country" })
        };

    public static IEnumerable<ApiResource> ApiResources =>
       new ApiResource[]
       {
             new ApiResource("imagegalleryapi", "Image Gallery Api", 
                 new [] { "role", "country" })
             {
                 Scopes = { "imagegalleryapi.fullaccess", "imagegalleryapi.write", "imagegalleryapi.read" }
             }
       };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { 
                new ApiScope("imagegalleryapi.fullaccess"),
                new ApiScope("imagegalleryapi.write"),
                new ApiScope("imagegalleryapi.read")
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            {
                new Client()
                {
                    ClientName= "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    //IdentityTokenLifetime = 
                    //AuthorizationCodeLifetime = 
                    AccessTokenLifetime = 120,
                    RedirectUris =
                    {
                        "https://localhost:7184/signin-oidc"
                    },
                    // scopes allowed to be requested by the client.
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7184/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        //"imagegalleryapi.fullaccess",
                        "imagegalleryapi.read",
                        "imagegalleryapi.write",
                        "country"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true
                }
            };
}