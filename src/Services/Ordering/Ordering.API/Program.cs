using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services 


            builder.Services.AddApplicationServices()
                            .AddInfrastructureServices(builder.Configuration)
                            .AddApiServices(builder.Configuration);


            #endregion


            var app = builder.Build();


            // Configure the HTTP request pipeline.

            #region Configure Middleware

            app.UseApiServices();

            if (app.Environment.IsDevelopment())
            {
                await app.InitializeDatabaseAsync();
            }

            #endregion

            app.Run();
        }
    }
}
