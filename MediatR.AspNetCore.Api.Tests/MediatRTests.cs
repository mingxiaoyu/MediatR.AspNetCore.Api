using MediatR.AspNetCore.Api.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QH.MediatR.AspNetCore.Api.Tests
{
    public class MediatRTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public MediatRTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Get_SecurePageRequiresAnAuthenticatedUser()
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync("/api/MediatR/a");
            var list = new List<string>() { "a", "Other" };

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();

            Assert.Equal(Newtonsoft.Json.JsonConvert.SerializeObject(list), json);
        }
    }
}
