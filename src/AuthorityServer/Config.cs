using Duende.IdentityServer.Models;

namespace AuthorityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(name: "apiName", displayName: "apiDisplayName"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { };
}