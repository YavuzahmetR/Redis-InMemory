using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
    }
}
