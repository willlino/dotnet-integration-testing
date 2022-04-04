using IntegrationTesting.API.Data.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IntegrationTesting.Tests.Support.SqlServer
{
    public class IntegrationTestWithSqlServer : IntegrationTest, IDisposable
    {
        public IntegrationTestingContext context;

        public IntegrationTestWithSqlServer(CustomWebAppFactory<API.Startup> httpTestFactory)
            : base(httpTestFactory)
        {
            context = serviceScope.ServiceProvider.GetRequiredService<IntegrationTestingContext>();
            RemoveAllDatabaseData();
        }

        public void Dispose()
        {
            RemoveAllDatabaseData();
            context.Dispose();
        }

        private void RemoveAllDatabaseData()
        {
            context.Customers.RemoveRange(context.Customers.ToList());

            context.SaveChanges();
        }
    }
}
