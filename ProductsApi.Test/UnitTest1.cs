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
                        // Mock the IProductRepository
                        var mockProductRepository = new Mock<IProductRepository>();
                        var testProducts = new List<Product>
                        {
                            new Product { ProductId = 1, ProductName = "Test Product 1", Description = "Test Desc 1", Price = 10.0m },
                            new Product { ProductId = 2, ProductName = "Test Product 2", Description = "Test Desc 2", Price = 20.0m }
                        };
                        mockProductRepository.Setup(repo => repo.GetAllProductsAsync()).Returns(Task.FromResult(testProducts));

                        // Register the mock repository
                        services.AddSingleton(mockProductRepository.Object);
                    });
                });

            _client = _factory.CreateClient();
        }

        [TestMethod]
        //get list of all products
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


[TestMethod]
//retrieve a product by id
public async Task GetProductById_ReturnsOkResult_WithProduct()
{
    // Arrange
    var mockProductRepo = new Mock<IProductRepository>();
    var testProduct = new Product
    {
        ProductId = 1,
        ProductName = "Test Product",
        Description = "Test Description",
        Price = 9.99m
    };

    mockProductRepo.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(testProduct);

    var factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IProductRepository>(mockProductRepo.Object);
            });
        });

    var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/products/1");

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    var returnedProduct = await response.Content.ReadFromJsonAsync<Product>();
    Assert.IsNotNull(returnedProduct);
    Assert.AreEqual(testProduct.ProductId, returnedProduct.ProductId);
    Assert.AreEqual(testProduct.ProductName, returnedProduct.ProductName);
}


 [TestMethod]

 //test for products not found
public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
{
    // Arrange
    var mockProductRepo = new Mock<IProductRepository>();
    mockProductRepo.Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

    var factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IProductRepository>(mockProductRepo.Object);
            });
        });

    var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/products/999");

    // Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
}


 [TestMethod]
 //test invalid routes
public async Task InvalidRoute_ReturnsNotFound()
{
    // Arrange
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/invalidroute");

    // Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
}


 [TestMethod]
 //test for empty products
public async Task GetAllProducts_ReturnsEmptyList_WhenNoProductsExist()
{
    // Arrange
    var mockProductRepo = new Mock<IProductRepository>();
    mockProductRepo.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(new List<Product>());

    var factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IProductRepository>(mockProductRepo.Object);
            });
        });

    var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/products");

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    var returnedProducts = await response.Content.ReadFromJsonAsync<List<Product>>();
    Assert.IsNotNull(returnedProducts);
    Assert.AreEqual(0, returnedProducts.Count);
}

        
    }
}
