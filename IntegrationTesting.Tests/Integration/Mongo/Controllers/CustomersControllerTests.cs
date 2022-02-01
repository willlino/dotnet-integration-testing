using FluentAssertions;
using IntegrationTesting.API.Models;
using IntegrationTesting.Tests.Support;
using IntegrationTesting.Tests.Support.Mongo;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting.Tests.Integration.Mongo.Controllers
{
    public class CustomersControllerTests : IntegrationTestWithMongo
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
        public async Task ShouldGetAllCustomers()
        {
            // Arrange
            var gabrielaNavarro = new Customer("Gabriela", "Navarro");
            var gabrielaFerraz = new Customer("Gabriela", "Ferraz");
            mongoCollection.InsertOne(gabrielaNavarro);
            mongoCollection.InsertOne(gabrielaFerraz);

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
        public async Task ShouldDeleteCustomer()
        {
            // Arrange
            var gabrielaNavarro = new Customer("Gabriela", "Navarro");
            var gabrielaFerraz = new Customer("Gabriela", "Ferraz");
            mongoCollection.InsertOne(gabrielaNavarro);
            mongoCollection.InsertOne(gabrielaFerraz);

            // Act
            var response = await httpClient.DeleteAsync($"api/customers/{gabrielaNavarro.Id}");

            // Assert
            response.Should()
                .Be200Ok()
                .And
                .MatchInContent("The customer was sucessfuly deleted");
                
            var queryResult = mongoCollection.Find(c => c.Id == gabrielaNavarro.Id).ToList();
            queryResult.Should().BeEmpty();
        }
    }
}
