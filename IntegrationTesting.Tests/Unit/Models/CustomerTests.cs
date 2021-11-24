using FluentAssertions;
using IntegrationTesting.API.Models;
using System;
using Xunit;

namespace IntegrationTesting.Tests.Unit.Models
{
    public class CustomerTests
    {
        [Fact(DisplayName = "Should constuct the customer correctly")]
        public void ShouldConstructCustomerCorrectly()
        {
            var customer = new Customer("João", "Farias");

            customer.Id.GetType().Should().Be<Guid>();
            customer.Id.Should().NotBeEmpty();
            customer.Name.Should().Be("João");
            customer.Surname.Should().Be("Farias");
        }
    }
}
