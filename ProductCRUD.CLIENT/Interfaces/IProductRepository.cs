using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Interfaces;
 
public interface IProductRepository
{
    Task<List<ProductModel>> GetAllProductsAsync(string token);
    Task<bool> AddProductAsync(AddProductDTO product, string token);
}