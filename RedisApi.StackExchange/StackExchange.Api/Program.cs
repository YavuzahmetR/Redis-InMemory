
using StackExchange.Api.Services;

namespace StackExchange.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<RedisService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
                try
                {
                    await redisService.ConnectAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Redis baðlantýsý baþarýsýz oldu: {ex.Message}");
                    throw;
                }
            }
            #region voidMain
            //using(var scope = app.Services.CreateScope())
            //{
            //    var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
            //    try
            //    {
            //         redisService.ConnectAsync().Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Redis baðlantýsý baþarýsýz oldu: {ex.Message}");
            //        throw;
            //    }
            //}
            #endregion

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
