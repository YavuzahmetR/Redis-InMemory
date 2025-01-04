using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);

    }
}
