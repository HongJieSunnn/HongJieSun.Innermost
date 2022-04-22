namespace Innermost.Identity.API.Configurations
{
    public class Config
    {
        /// <summary>
        /// Apis in Innermost
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("loglife","LogLife Service Api")
                {
                    Scopes={"loglife"},
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("meet","Meet Service Api")
                {
                    Scopes={ "meet" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("musichub","MusicHub Service Api")
                {
                    Scopes={ "musichub" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("filesystem","FileSystem Service Api")
                {
                    Scopes={ "filesystem" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("intelligence","Intelligence Service Api")
                {
                    Scopes={ "intelligence" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("push","Push Service Api")
                {
                    Scopes={ "push" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },
                new ApiResource("tagserver","TagServer Service Api")
                {
                    Scopes={ "tagserver" },
                    UserClaims=new[]{ ClaimTypes.Role }
                },

            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("loglife"),
                new ApiScope("meet"),
                new ApiScope("musichub"),
                new ApiScope("filesystem"),
                new ApiScope("intelligence"),
                new ApiScope("push"),
                new ApiScope("tagserver"),
            };
        }

        /// <summary>
        /// Get IdentityResource like userID、Profile、email which needs to be protected.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource("userstatue",new[]{"user_statue"})
            };
        }
        /// <summary>
        /// Clients want to access resources
        /// </summary>
        /// <param name="clientUrls">clients' url dictionary key:clientId value:url.The data is in appsettings.json</param>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="webapp",
                    ClientName="Web FrontEnd Of Innermost",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=false,//needn't consent screen (consent what profile you agree to be accessed)
                    RedirectUris={ "http://localhost:3000/signin-oidc"/*$"{clientUrls["WebApp"]}/"*/},
                    PostLogoutRedirectUris={$"{clientUrls["WebApp"]}/"},
                    AllowedCorsOrigins={$"http://localhost:3000" },/*{clientUrls["WebApp"]}*/
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "userstatue",
                        "loglife",
                        "meet",
                        "musichub",
                        "tagserver",
                    },
                    AccessTokenLifetime=60*60*24,
                    IdentityTokenLifetime=60*60*24,
                    AlwaysIncludeUserClaimsInIdToken=true,
                },
                new Client
                {
                    ClientId="reactapp",
                    ClientName="Innermost React App",
                    ClientUri="http://localhost:3000",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireClientSecret=false,
                    RedirectUris={$"{clientUrls["ReactApp"]}"},
                    PostLogoutRedirectUris={$"{clientUrls["ReactAppLogout"]}"},
                    AllowedCorsOrigins={$"http://localhost:3000" },
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "userstatue",
                        "loglife",
                        "meet",
                        "musichub",
                        "tagserver",
                    },
                    AccessTokenLifetime=60*60*24,
                    IdentityTokenLifetime=60*60*24,
                    AlwaysIncludeUserClaimsInIdToken=true
                },
                new Client
                {
                    ClientId="mobileapp",
                    ClientName="Mobile App Of Innermost",
                    ClientSecrets={new Secret("mobileappInnermost".Sha256())},
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser=false,
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=60*60*24,
                    IdentityTokenLifetime=60*60*24,
                    RequireConsent=false,
                    RequirePkce=true,
                    RedirectUris = { clientUrls["MobileApp"] },
                    PostLogoutRedirectUris = { clientUrls["MobileApp"] },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "loglife",
                        "meet",
                        "musichub",
                        "tagserver",
                    },
                },
                new Client
                {
                    ClientId="serviceclient",
                    ClientName="Service Client",
                    ClientSecrets={new Secret("service-client".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser=false,
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=60*60*24*30,
                    IdentityTokenLifetime=60*60*24*30,
                    AllowedScopes = new List<string>
                    {
                        "loglife",
                        "meet",
                        "musichub",
                        "filesystem",
                        "intelligence",
                        "push",
                        "tagserver",
                    },
                },
            };
        }
    }
}
