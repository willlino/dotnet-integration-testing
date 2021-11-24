using IntegrationTesting.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace IntegrationTesting.API.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> customersCollection;

        public CustomerRepository(IMongoConfiguration mongoConfiguration)
        {
            this.customersCollection = mongoConfiguration.GetDatabase().GetCollection<Customer>("customers");
        }

        public async Task AddOrUpdate(Customer customer)
        {

            await this.customersCollection.ReplaceOneAsync(
                c => c.Id == customer.Id,
                customer,
                new ReplaceOptions { IsUpsert = true });
        }

        public async Task<List<Customer>> GetAll()
        {
            var customers = (await this.customersCollection.FindAsync(_ => true)).ToList();

            return customers;
        }

        public async Task<Customer> Get(Guid id)
        {
            var customer = (await this.customersCollection.FindAsync(c => c.Id == id)).SingleOrDefault();

            return customer;
        }

        public async Task Delete(Guid id)
        {
            await this.customersCollection.FindOneAndDeleteAsync(c => c.Id == id);
        }
    }
}
