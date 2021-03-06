using FluentAssertions;
using IntegrationTesting.API.Models;
using IntegrationTesting.Tests.Support;
using IntegrationTesting.Tests.Support.SqlServer;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting.Tests.Integration.SqlServer.Controllers
{
    public class CustomersControllerTests : IntegrationTestWithSqlServer
    {
        public CustomersControllerTests(CustomWebAppFactory<API.Startup> httpTestFactory) : base(httpTestFactory)
        {
        }

        [Fact(DisplayName = "Should add customer")]
        public async Task PostCustomers_ShouldAddCustomerAndReturn200Ok_WhenCustomerDoesNotExistsInDatabase()
        {
            // Arrange
            var customer = new Customer("Gabriela", "Navarro");

            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync("api/customers", content);

            // Assert
            response.Should().Be200Ok();
            var dbCustomer = context.Customers.SingleOrDefault(x => x.Id == customer.Id);
            dbCustomer.Should().BeEquivalentTo<Customer>(customer);

        }

        [Fact(DisplayName = "Should update customer")]
        public async Task PostCustomers_ShouldUpdateCustomerAndReturn200Ok_WhenCustomerExistsInDatabase()
        {
            // Arrange
            var customer = new Customer("Gabriela", "Navarro");
            context.Customers.Add(customer);
            context.SaveChanges();

            customer = new Customer("Gabriela", "Ferraz");
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync("api/customers", content);

            // Assert
            response.Should().Be200Ok();
            var dbCustomer = context.Customers.SingleOrDefault(x => x.Id == customer.Id);
            dbCustomer.Should().BeEquivalentTo<Customer>(customer);
        }

        [Fact(DisplayName = "Should get all customers")]
        public async Task GetCustomers_ShouldGetAllCustomers()
        {
            // Arrange
            var gabrielaNavarro = new Customer("Gabriela", "Navarro");
            var gabrielaFerraz = new Customer("Gabriela", "Ferraz");
            context.Customers.Add(gabrielaNavarro);
            context.Customers.Add(gabrielaFerraz);
            context.SaveChanges();

            // Act
            var response = await httpClient.GetAsync("api/customers");

            // Assert
            response.Should()
                .Be200Ok()
                .And
                .Satisfy<List<Customer>>(content =>
                {
                    content.Should().HaveCount(2);
                    content.Should().ContainSingle(x => x.Id == gabrielaNavarro.Id);
                    content.Should().ContainSingle(x => x.Id == gabrielaFerraz.Id);
                });
        }

        [Fact(DisplayName = "Should delete customer by id")]
        public async Task DeleteCustomers_ShouldDeleteCustomer_WhenCustomerExistsInDatabase()
        {
            // Arrange
            var gabrielaNavarro = new Customer("Gabriela", "Navarro");
            var gabrielaFerraz = new Customer("Gabriela", "Ferraz");
            context.Customers.Add(gabrielaNavarro);
            context.Customers.Add(gabrielaFerraz);
            context.SaveChanges();

            // Act
            var response = await httpClient.DeleteAsync($"api/customers/{gabrielaNavarro.Id}");

            // Assert
            response.Should()
                .Be200Ok()
                .And
                .MatchInContent("The customer was sucessfuly deleted");

            var queryResult = context.Customers.Where(c => c.Id == gabrielaNavarro.Id);
            queryResult.Should().BeEmpty();
        }
    }
}
