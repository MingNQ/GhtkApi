using ClientAuthentication;
using Ghtk.Api.AuthenticationHandler;
using Ghtk.Authorization;

namespace Ghtk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IClientSourceAuthenticationHandler clientSourceAuthenticationHandler = new RemoteClientSourceAuthenticationHandler(builder.Configuration["AuthenticationService"] ?? throw new Exception("AuthenticationService url not found!"));

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("X-Client-Source").AddXClientSource(options =>
            {
                options.ClientValidator = (clientSource, token, principal) => clientSourceAuthenticationHandler.Validate(clientSource);
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
