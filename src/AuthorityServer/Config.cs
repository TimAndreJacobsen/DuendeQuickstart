using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace AuthorityServer;

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
            new ApiScope(name: "apiName", displayName: "apiDisplayName"),
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
                AllowedScopes = { "apiName" }
            },
            new Client
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
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                }
                
            }
        };
}