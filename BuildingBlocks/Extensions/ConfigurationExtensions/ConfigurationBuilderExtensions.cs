namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfiguration BuildLocalConfiguration(this ConfigurationBuilder configurationBuilder, string basePath)
        {
            var config = configurationBuilder
                        .SetBasePath(basePath)
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();

            return config;
        }
    }
}
