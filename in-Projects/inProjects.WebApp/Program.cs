using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new WebHostBuilder()
                            .UseUrls( "http://localhost:5000", "http://*:8080" )
                            .UseContentRoot( Directory.GetCurrentDirectory() )
                            .UseMonitoring()
                            .UseScopedHttpContext()
                            .ConfigureAppConfiguration( ( hostBuilderContext, confBuilder ) =>
                            {
                                confBuilder
                                    .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
                                    .AddJsonFile( $"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true )
                                    .AddEnvironmentVariables();


                            } )
                            .UseKestrel()
                            .UseIIS()
                            .UseStartup<Startup>();
            builder.Build().Run();
        }
    }
}
