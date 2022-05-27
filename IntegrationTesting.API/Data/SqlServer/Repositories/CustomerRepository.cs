using IntegrationTesting.API.Data.RepositoriesInterfaces;
using IntegrationTesting.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTesting.API.Data.SqlServer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        IntegrationTestingContext context;
        public CustomerRepository(IntegrationTestingContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdate(Customer customer)
        {
            if (this.context.Customers.Any(c => c.Id == customer.Id))
                this.context.Entry(customer).State = EntityState.Modified;
            else
                this.context.Entry(customer).State = EntityState.Added;

            await this.context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var customer = await this.context.Customers.SingleAsync(c => c.Id == id);
            this.context.Customers.Remove(customer);

            await this.context.SaveChangesAsync();
        }

        public async Task<Customer> Get(Guid id)
        {
            var customer = await this.context.Customers.SingleOrDefaultAsync(c => c.Id == id);
            return customer;
        }

        public async Task<List<Customer>> GetAll()
        {
            return await this.context.Customers.ToListAsync();
        }
    }
}
