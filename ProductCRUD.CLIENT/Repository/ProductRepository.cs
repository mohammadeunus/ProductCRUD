using Newtonsoft.Json;
using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Interfaces;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly HttpClient _client;

    public ProductRepository(ILogger<ProductRepository> logger)
    {
        _logger = logger;
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://stg-zero.propertyproplus.com.au");
    }

    public async Task<List<ProductModel>> GetAllProductsAsync()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync("/api/services/app/ProductSync/GetAllproduct");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<ProductModel>>(await response.Content.ReadAsStringAsync());
            }

            return new List<ProductModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError("ProductRepository > GetAllProductsAsync Error: " + ex.ToString());
            return new List<ProductModel>();
        }
    }

    public async Task<bool> AddProductAsync(AddProductDTO product)
    {
        try
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/services/app/ProductSync/CreateOrEdit", product);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("ProductRepository > AddProductAsync Error: " + ex.ToString());
            return false;
        }
    }
}
