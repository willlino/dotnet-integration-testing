using System;

namespace IntegrationTesting.API.Models
{
    public class Customer
    {
        public Customer(string name, string surname)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
