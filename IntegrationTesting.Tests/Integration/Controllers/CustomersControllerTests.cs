using FluentAssertions;
using IntegrationTesting.API.Models;
using IntegrationTesting.Tests.Support;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting.Tests.Integration.Controllers
{
    public class CustomersControllerTests : IntegrationTest
    {
        private readonly IMongoCollection<Customer> mongoCollection;
        public CustomersControllerTests(CustomWebAppFactory<API.Startup> httpTestFactory) : base(httpTestFactory)
        {
            mongoCollection = mongoDatabase.GetCollection<Customer>("customers");
        }

        [Fact(DisplayName = "Should add customer")]
        public async Task ShouldAddCustomer()
        {
            // Arrange
            var customer = new Customer("Gabriela", "Navarro");

            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync("api/customers", content);

            // Assert
            response.Should().Be200Ok();
            var dbCustomer = (await mongoCollection.FindAsync(x => x.Id == customer.Id)).SingleOrDefault();
            dbCustomer.Should().BeEquivalentTo<Customer>(customer);

        }

        [Fact(DisplayName = "Should update customer")]
        public async Task ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer("Gabriela", "Navarro");
            mongoCollection.InsertOne(customer);
            var gabriela = (await mongoCollection.FindAsync(x => x.Id == customer.Id)).SingleOrDefault();

            customer = new Customer("Gabriela", "Ferraz");
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync("api/customers", content);

            // Assert
            response.Should().Be200Ok();
            var dbCustomer = (await mongoCollection.FindAsync(x => x.Id == customer.Id)).SingleOrDefault();
            dbCustomer.Should().BeEquivalentTo<Customer>(customer);
        }

        [Fact(DisplayName = "Should get all customers")]
        public void ShouldGetAllCustomers()
        {
        }

        [Fact(DisplayName = "Should delete customer by id")]
        public void ShouldDeleteCustomer()
        {
        }

        /// <summary>
        /// This test is only to see if DI would work on integration tests and when the app is running
        /// </summary>
        [Fact(DisplayName = "Should return string in test DI")]
        public async void TestDI_ShouldDeleteCustomer()
        {
            var result = await httpClient.GetAsync("api/customers/TestDI");

            result
                .Should()
                .Be200Ok()
                .And
                .MatchInContent("Test DI worked!");
        }
    }
}
