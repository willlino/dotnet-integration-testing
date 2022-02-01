using IntegrationTesting.API.AppServices;
using IntegrationTesting.API.Data.Mongo;
using IntegrationTesting.API.Data.RepositoriesInterfaces;
using IntegrationTesting.API.Data.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace IntegrationTesting.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMongoConfiguration, MongoConfiguration>();
            services.AddDbContext<IntegrationTestingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase")));

            // App Services
            services.AddScoped<ITestAppService, TestAppService>();

            // Repositories
            //services.AddScoped<ICustomerRepository, Data.Mongo.Repositories.CustomerRepository>();
            services.AddScoped<ICustomerRepository, Data.SqlServer.Repositories.CustomerRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegrationTesting.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //if(env.EnvironmentName == "Testing")
            //{
                var sqlServerContext = serviceProvider.GetRequiredService<IntegrationTestingContext>();
                sqlServerContext.Database.Migrate();
            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IntegrationTesting.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
