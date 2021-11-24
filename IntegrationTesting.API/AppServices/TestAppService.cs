namespace IntegrationTesting.API.AppServices
{
    public class TestAppService : ITestAppService
    {
        public string MethodToTestDI()
        {
            return "Test DI worked!";
        }
    }
}
