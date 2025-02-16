using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256()) },
                    AllowedScopes = {"movieApi"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[] {
            new ApiScope("movieApi","MovieAPI")
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { };

        public static IEnumerable<TestUser> TestUsers => new TestUser[] { };
    }
}
