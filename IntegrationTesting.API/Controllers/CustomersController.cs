using IntegrationTesting.API.AppServices;
using IntegrationTesting.API.Data.Repositories;
using IntegrationTesting.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IntegrationTesting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ITestAppService testAppService;
        
        public CustomersController(ICustomerRepository customerRepository, ITestAppService testAppService)
        {
            this.customerRepository = customerRepository;
            this.testAppService = testAppService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await this.customerRepository.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await this.customerRepository.Get(id);

            return Ok(customer);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            await this.customerRepository.AddOrUpdate(customer);
            return Ok("The customer was added/updated");
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.customerRepository.Delete(id);
            return Ok("The customer was sucessfuly deleted");
        }
    }
}
