using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // If you're using Newtonsoft.Json for deserialization

namespace ProductsApi.Services // Change the namespace to something more descriptive
{
    public class ThirdPartyProductService
    {
        private readonly HttpClient _httpClient;

    
        public ThirdPartyProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //  fetch products from a third-party API
        public async Task<List<Product>> GetProductsFromApiAsync(string apiUrl)
        {
        
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch products from third-party API");
            }

           
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);

            return products;
        }
    }
}
