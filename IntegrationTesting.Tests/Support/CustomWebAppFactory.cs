using IntegrationTesting.API.Data.Mongo;
using IntegrationTesting.API.Data.SqlServer;
using IntegrationTesting.Tests.Support.Mongo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
            var config = SetConfiguration();

            builder
                .UseConfiguration(config);

            builder.ConfigureServices(serviceCollection =>
            {
                MongoInjection(serviceCollection);
                //SqlServerInjection(serviceCollection, config);
            });

            base.ConfigureWebHost(builder);
        }

        private static IConfiguration SetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var env = "Testing";
            if (env != null) configurationBuilder.AddJsonFile($"appsettings.{env}.json");
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private static void MongoInjection(IServiceCollection serviceCollection)
        {
            var currentMongoConfiguration = serviceCollection.SingleOrDefault(s => s.ServiceType == typeof(IMongoConfiguration));
            serviceCollection.Remove(currentMongoConfiguration);
            serviceCollection.AddSingleton<IMongoConfiguration, MongoTestConfiguration>();
        }

        private static void SqlServerInjection(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var currentSqlServerConfiguration = serviceCollection.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<IntegrationTestingContext>));
            serviceCollection.Remove(currentSqlServerConfiguration);
            serviceCollection.AddDbContext<IntegrationTestingContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlDatabase")));
        }
    }
}
