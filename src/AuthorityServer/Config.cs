using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace AuthorityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            // Weather and Identity listing api
            new ApiScope(name: "weatherApi", displayName: "apiDisplayName"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client // minimal machine to machine (m2m) conf
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "weatherApi" }
            },
            new Client // WebClient app
            {
                // Client name and display name
                ClientId = "webapp",
                ClientName = "WebApp",

                // Set up super secret and non-guessable secret
                ClientSecrets = { new Secret("secret".Sha256()) },

                // Allowed grant types
                AllowedGrantTypes = GrantTypes.Code,

                // Where to redirect after login
                RedirectUris = { "https://localhost:5002/signin-oidc" },

                // Where to redirect after logout
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "verification",
                    "weatherApi"
                }
            },
            new Client // JavaScript BFF client
            {
                ClientId = "jsBff",
                ClientName = "JS with BFF Web App",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5003/signin-oidc" },

                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "weatherApi"
                },
            }
        };
}