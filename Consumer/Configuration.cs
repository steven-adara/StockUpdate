using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace Kfzteile24.Interfaces.StockUpdate.Consumer
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