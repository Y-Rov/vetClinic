using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
        => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResource(
                name: "profile",
                userClaims: new []{"name"},
                displayName: "Profile data about user")
        };

    public static IEnumerable<ApiResource> GetApiResources()
        => new List<ApiResource>()
        {
            new ApiResource("vetClinicApi", "Vet clinic API")
            {
                Scopes = {"apiAccess"}
            } 
        };
    
    public static IEnumerable<ApiScope> GetApiScopes()
        =>  new[]
        {
            new ApiScope("apiAccess", "Access vet clinic Api")
        };

    public static IEnumerable<Client> GetClients()
        => new[]
        {
            // dev client that has access to all scopes of api (for tests in postman etc.)
            new Client()
            {
                RequireConsent = false,
                ClientId = "postman_client",
                ClientName = "Postman Client",
                AllowedScopes = {"apiAccess", "openid", "profile"},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true, // enable refresh tokens
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets = {new Secret("tests_client_secret".ToSha256())},
                AccessTokenLifetime = 600,
            },
            
            // angular client
            new Client()
            {
                ClientId = "angular_client",
                ClientName = "Angular Client",
                AllowedScopes = {"apiAccess", "openid", "profile"},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true, // enable refresh tokens
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets = {new Secret("angular_client_secret".ToSha256())},
                AllowedCorsOrigins = new [] {"http://localhost:4200"},
                AccessTokenLifetime = 600,
            }
        };
}