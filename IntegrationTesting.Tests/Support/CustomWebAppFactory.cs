using IntegrationTesting.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IntegrationTesting.Tests.Support
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            builder
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = "Testing";
                    if (env != null) config.AddJsonFile($"appsettings.{env}.json");

                    config.AddEnvironmentVariables();
                    config.Build();
                });

            builder.ConfigureServices(serviceCollection =>
            {
                var currentMongoConfiguration = serviceCollection.SingleOrDefault(s => s.ServiceType == typeof(IMongoConfiguration));
                serviceCollection.Remove(currentMongoConfiguration);
                serviceCollection.AddSingleton<IMongoConfiguration, MongoTestConfiguration>();
            });

            base.ConfigureWebHost(builder);
        }
    }
}
