using project_backend.Controllers;
using project_backend.Models;
using System;
using System.Linq;
using Xunit;

namespace project_backend.Test
{
    [Trait("Category", "Unit")]
    public class WeatherForecastControllerTest
    {
        WeatherForecastController _controller;
        //private readonly TestServer _server;
        //private readonly HttpClient _client;


        public WeatherForecastControllerTest()
        {
            //_server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            //_client = _server.CreateClient();
            _controller = new WeatherForecastController(null, null);
            Environment.SetEnvironmentVariable("JWT_ISSUER", "https://localhost:44309");
            Environment.SetEnvironmentVariable("JWT_KEY", "very secret indeed");
        }

        [Fact]
        public void TestGet()
        {
            // Arrange
            Token result;

            // Act
            result = _controller.Get();

            // Assert
            Assert.True(result.token.Length > 0);
        }

        [Fact]
        public void TestGetStuff()
        {
            // Arrange
            // Act
            var result = _controller.GetStuff().ToList();

            // Assert
            Assert.True(result.Count == 5);
        }
    }
}
