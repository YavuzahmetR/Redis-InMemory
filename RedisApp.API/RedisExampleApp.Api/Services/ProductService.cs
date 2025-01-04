using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;

namespace RedisExampleApp.Api.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<Product> CreateProductAsync(Product product)
        {
            return await productRepository.CreateProductAsync(product);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
           var value = await productRepository.GetProductByIdAsync(id);
            //mapperdto
            return value;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await productRepository.GetProductsAsync();
        }
    }
}
