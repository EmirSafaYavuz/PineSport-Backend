using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using WebAPI;

namespace Tests.Helpers.Token
{
    [TestFixture]
    public abstract class BaseIntegrationTest : WebApplicationFactory<Program>
    {
        private static readonly JwtSecurityTokenHandler s_TokenHandler = new ();

        public string Issuer { get; } = "www.pinesport.com";
        public string Audience { get; } = "www.pinesport.com";
        public SigningCredentials SigningCredentials { get; }

        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// Optionally create a custom factory so you can override services if you need 
        /// an in-memory DB or mocks instead of real services.
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // You can change environment to "Test".
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Example: remove real DB context if you have one, add in-memory DB context, 
                // or register mocks for external dependencies, etc.

                // var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                // if (descriptor != null) services.Remove(descriptor);
                // services.AddDbContext<AppDbContext>(options =>
                // {
                //     options.UseInMemoryDatabase("InMemoryTestDb");
                // });
            });
        }

        [SetUp]
        public void Setup()
        {
            HttpClient = CreateClient();
        }
    }
}