using Carter;

namespace CatalogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Services to the Container 
            #region configure Services

            builder.Services.AddCarter();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            #endregion


            var app = builder.Build();




            // Configure the Http request pipeline
            #region Configure Middleware
            
            app.MapCarter();

            #endregion

            app.Run();
        }
    }
}
