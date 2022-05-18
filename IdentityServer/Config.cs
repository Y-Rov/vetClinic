using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
        => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> GetApiResources()
        => new List<ApiResource>()
        {
            new ApiResource("vetClinicApi", "Vet clinic API") 
        };

    public static IEnumerable<Client> GetClients()
        => new[]
        {
            // dev client that has access to all scopes of api (for tests in postman etc.)
            new Client()
            {
                RequireConsent = false,
                ClientId = "postman_client",
                ClientName = "Postman",
                AllowedScopes = {"vetClinicApi"},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true, // enable refresh tokens
                ClientSecrets = {new Secret("tests_client_secret".ToSha256())},
                AccessTokenLifetime = 3600,
            },
            
            // angular client
            new Client()
            {
                ClientId = "angular_client",
                ClientName = "Angular Client",
                AllowedScopes = {"vetClinicApi"},
                RequireConsent = false,
                RequireClientSecret = false,
                AllowOfflineAccess = true, // enable refresh tokens
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RedirectUris =           { "http://localhost:4200" },
                PostLogoutRedirectUris = { "http://localhost:4200" },
                AllowedCorsOrigins =     { "http://localhost:4200" },
                AccessTokenLifetime = 3600,
            }
        };
}