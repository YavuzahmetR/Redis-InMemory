using RedisExampleApp.Api.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Api.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string CacheKey = "Products";
        private readonly IProductRepository _productRepository;
        private readonly IDatabaseAsync _database;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, IDatabaseAsync database)
        {
            _productRepository = productRepository;
            _database = database;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var newProduct = await _productRepository.CreateProductAsync(product);

            if(await _database.KeyExistsAsync(CacheKey))
            {
                await _database.HashSetAsync(CacheKey, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            if(await _database.KeyExistsAsync(CacheKey))
            {
                var product = await _database.HashGetAsync(CacheKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product!)! : await _productRepository.GetProductByIdAsync(id);
            }
            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            if (!await _database.KeyExistsAsync(CacheKey))
            {
                return await LoadToCacheFromDbAsync();
            }

            var products = new List<Product>();

            var cachedProducts = await _database.HashGetAllAsync(CacheKey);
            foreach (var product in cachedProducts.ToList())
            {
                products.Add(JsonSerializer.Deserialize<Product>(product.Value!)!);
            }
            return products;
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetProductsAsync();

            var tasks = products.Select(product => _database.HashSetAsync(CacheKey,product.Id,JsonSerializer.Serialize(product)));
            await Task.WhenAll(tasks);
            return products;

        }
    }
}
