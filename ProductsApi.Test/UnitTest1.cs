using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using ProductsApi;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsApiTest
{
    [TestClass]
    public class ProductsApiTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ProductsApiTests()
        {
            // Set up the WebApplicationFactory and HttpClient
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Mock the IProductRepository and replace it in the DI container
                        var mockProductRepository = new Mock<IProductRepository>();
                        var testProducts = new List<Product>
                        {
                            new Product { ProductId = 1, ProductName = "Test Product 1", Description = "Test Desc 1", Price = 10.0m },
                            new Product { ProductId = 2, ProductName = "Test Product 2", Description = "Test Desc 2", Price = 20.0m }
                        };
                        mockProductRepository.Setup(repo => repo.GetAllProductsAsync()).Returns(Task.FromResult(testProducts));

                        // Register the mock repository to replace the actual one
                        services.AddSingleton(mockProductRepository.Object);
                    });
                });

            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Act: Send a GET request to /products
            var response = await _client.GetAsync("api/products");

            // Assert: Check that the response status is OK and matches the expected list of products
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var returnedProducts = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.IsNotNull(returnedProducts);
            Assert.AreEqual(2, returnedProducts.Count);
        }
    }
}
