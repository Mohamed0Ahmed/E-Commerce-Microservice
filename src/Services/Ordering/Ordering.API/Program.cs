using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services 



            builder.Services
                .AddApplicationServices()
                .AddInfrastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);

            #endregion


            var app = builder.Build();


            // Configure the HTTP request pipeline.

            #region Configure Middleware


            #endregion

            app.Run();
        }
    }
}
