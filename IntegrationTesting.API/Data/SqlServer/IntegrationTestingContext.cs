using IntegrationTesting.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTesting.API.Data.SqlServer
{
    public class IntegrationTestingContext : DbContext
    {
        public IntegrationTestingContext(DbContextOptions<IntegrationTestingContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
        }
    }
}
