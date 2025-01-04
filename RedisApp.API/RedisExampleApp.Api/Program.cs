
using Microsoft.EntityFrameworkCore;
using RedisConnect;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;
using RedisExampleApp.Api.Services;
using StackExchange.Redis;

namespace RedisExampleApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<RedisService>(sp =>
            {
                return new RedisService(builder.Configuration["CacheOptions:Url"]!);
            });

            builder.Services.AddSingleton<IDatabaseAsync>(sp =>
            {
                var redisService = sp.GetRequiredService<RedisService>();
                return redisService.GetDatabaseAsync(1);
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("RedisDb");
            });

            builder.Services.AddScoped<IProductRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<AppDbContext>();

                var productRepository = new ProductRepository(dbContext);

                var database = sp.GetRequiredService<IDatabaseAsync>();

                return new ProductRepositoryWithCacheDecorator(productRepository, database);
            });


            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                appDbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();


            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
