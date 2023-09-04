using ProductCRUD.CLIENT.DTOs;
using ProductCRUD.CLIENT.Models;

namespace ProductCRUD.CLIENT.Interfaces;
 
public interface IProductRepository
{
    Task<List<ProductModel>> GetAllProductsAsync();
    Task<bool> AddProductAsync(AddProductDTO product);
}