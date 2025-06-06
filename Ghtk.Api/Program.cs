using Ghtk.Authorization;

namespace Ghtk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("X-Client-Source").AddXClientSource(options =>
            {
                options.ClientValidator = (clientSource, token, principal) => true;
                options.IssuerSigningKey = builder.Configuration["IssuerSigningKey"] ?? "";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
