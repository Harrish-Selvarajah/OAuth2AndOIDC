// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Harrish.IDP;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {       
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Chandler",
                    Password = "password",
                    Claims =
                    {
                        new Claim("role", "FreeUser"),
                        new Claim(JwtClaimTypes.GivenName, "Chandler"),
                        new Claim(JwtClaimTypes.FamilyName, "Bing"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Monica",
                    Password = "password",
                    Claims =
                    {
                        new Claim("role", "PayingUser"),
                        new Claim(JwtClaimTypes.GivenName, "Monica"),
                        new Claim(JwtClaimTypes.FamilyName, "Gellar"),
                    }
                },
            };
        }
    }
}