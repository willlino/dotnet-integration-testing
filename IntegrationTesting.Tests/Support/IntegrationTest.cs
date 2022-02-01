using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace IntegrationTesting.Tests.Support
{
    public class IntegrationTest : IClassFixture<CustomWebAppFactory<API.Startup>>
    {
        public readonly HttpClient httpClient;
        public readonly IServiceScope serviceScope;

        public IntegrationTest(CustomWebAppFactory<API.Startup> httpTestFactory)
        {
            var _httpTestFactory = httpTestFactory;

            httpClient = httpTestFactory.CreateClient();
            serviceScope = _httpTestFactory.Services.CreateScope();
        }
    }
}
