using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
           var value = await _context.Products.FindAsync(id);
           if(value == null)
            {
                throw new Exception("Value Not Found");
            }
            return value;

        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
