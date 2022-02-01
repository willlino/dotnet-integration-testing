using IntegrationTesting.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationTesting.API.Data.RepositoriesInterfaces
{
    public interface ICustomerRepository
    {
        Task AddOrUpdate(Customer customer);
        Task Delete(Guid id);
        Task<Customer> Get(Guid id);
        Task<List<Customer>> GetAll();
    }
}