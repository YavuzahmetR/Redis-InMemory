using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<RedisService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            using (var scope = app.Services.CreateScope())
            {
                var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
                try
                {
                    await redisService.ConnectAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Redis ba�lant�s� ba�ar�s�z oldu: {ex.Message}");
                    throw;
                }
            }
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}