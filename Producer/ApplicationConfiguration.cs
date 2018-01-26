using System;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Kfzteile24.Interfaces.StockUpdate.Producer
{
    public class ApplicationConfiguration
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Configure()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Appconfig.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }

}