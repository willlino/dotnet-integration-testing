using FluentAssertions;
using IntegrationTesting.API.Models;
using System;
using Xunit;

namespace IntegrationTesting.Tests.Unit.Models
{
    public class CustomerTests
    {
        [Fact(DisplayName = "Should construct the customer correctly")]
        public void Should_CustomerConstructor_ConstructCustomerCorrectly()
        {
            // Arrange and Act
            var customer = new Customer("João", "Farias");

            // Assert
            customer.Id.GetType().Should().Be<Guid>();
            customer.Id.Should().NotBeEmpty();
            customer.Name.Should().Be("João");
            customer.Surname.Should().Be("Farias");
        }
    }
}
