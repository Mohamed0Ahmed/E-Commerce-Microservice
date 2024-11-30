using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Marten;

namespace Basket.Api
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
                config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            builder.Services.AddMarten(option =>
            {
                option.Connection(builder.Configuration.GetConnectionString("Database")!);
                option.Schema.For<ShoppingCart>().Identity(S => S.UserName);
            }).UseLightweightSessions();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            #endregion


            var app = builder.Build();

            // Configure the Http request pipeline

            #region Configure Middleware

            app.MapCarter();
            app.UseExceptionHandler(options => { });

            #endregion

            app.Run();
        }
    }
}
