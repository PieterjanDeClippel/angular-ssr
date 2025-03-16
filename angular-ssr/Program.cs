
namespace angular_ssr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseHealthChecks("/healthz");
            app.MapGet("/", (context) =>
            {
                context.Response.Redirect("/WeatherForecast");
                return Task.CompletedTask;
            });
            app.MapControllers();

            app.Run();
        }
    }
}
