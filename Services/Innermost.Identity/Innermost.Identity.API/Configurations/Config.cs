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
                    Scopes={"loglife"}
                },
                new ApiResource("meet","Meet Service Api")
                {
                    Scopes={ "meet" }
                },
                new ApiResource("tagserver","TagServer Service Api")
                {
                    Scopes={ "tagserver" }
                },
                
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("loglife"),
                new ApiScope("meet"),
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
                new IdentityResources.Profile()
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
                        "loglife",
                        "meet",
                        "tagserver",
                    },
                    AccessTokenLifetime=60*60*3,
                    IdentityTokenLifetime=60*60*3,
                    AlwaysIncludeUserClaimsInIdToken=true
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
                        "loglife",
                        "meet",
                        "tagserver",
                    },
                    AccessTokenLifetime=60*60*12,
                    IdentityTokenLifetime=60*60*12,
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
                    AccessTokenLifetime=60*60*2,
                    IdentityTokenLifetime=60*60*2,
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
                        "tagserver",
                    },
                }
            };
        }
    }
}
