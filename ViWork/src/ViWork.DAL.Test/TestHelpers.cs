using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ViWork.DAL.Test
{
    public static class TestHelpers
    {
        static readonly Random _random = new Random();
        static IConfiguration _configuration;

        public static string ConnectionString
        {
            get
            {
                return Configuration["ConnectionStrings:ViWorkDB"];
            }
        }

        static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
                }

                return _configuration;
            }
        }

        public static string RandomTestName() => string.Format("Test-{0}", Guid.NewGuid().ToString().Substring(24));

    }
}
