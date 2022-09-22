using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityModel;

namespace FW.Identity
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("FWWebAPI", "Web API")
            {
                Scopes = {"scopeWebAPI"},
                UserClaims =
                {
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Expiration,
                    JwtClaimTypes.Audience,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.Scope,
                    JwtClaimTypes.AuthenticationMethod
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("scopeWebAPI", "Web API")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "clientWebApi",
                ClientSecrets = { new Secret("36a4d0df-d361-4c3c-a3eb-2e519d4c4391".ToSha256())},
                AllowedGrantTypes =  GrantTypes.ResourceOwnerPassword,
                //AllowAccessTokensViaBrowser = true,
                //AlwaysSendClientClaims = true,
                //AlwaysIncludeUserClaimsInIdToken = true,
                //AccessTokenType = AccessTokenType.Jwt,
                AllowedCorsOrigins = { "https://localhost:2001" },
                AllowedScopes =
                {
                    "scopeWebAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                }
            },
            new Client
            {
                ClientId = "clientConsole",
                ClientSecrets = { new Secret("42467dee-f65d-481c-9508-74891854ddaa".ToSha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "scopeWebAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
             new Client
            {
                ClientId = "clientWpf",
                ClientSecrets = { new Secret("45467dee-f65d-481c-9508-74891854ddaa".ToSha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "scopeWebAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
        };
    }
}
