using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService
{
    public class ConfigurationService
    {
        public static IConfiguration GetConfiguration(string basePath)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
            return config;
        }
    }
}
