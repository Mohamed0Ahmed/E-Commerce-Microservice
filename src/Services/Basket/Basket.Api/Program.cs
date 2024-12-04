using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Basket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Services to the Container 

            #region configure Services

            // Application Services
            builder.Services.AddCarter();
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            //Data Services
            builder.Services.AddMarten(option =>
            {
                option.Connection(builder.Configuration.GetConnectionString("Database")!);
                option.Schema.For<ShoppingCart>().Identity(S => S.UserName);
            }).UseLightweightSessions();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            //Grpc Services
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                return handler;
            });

            // Cross Cutting Services
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddHealthChecks()
                 .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                 .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

            #endregion


            var app = builder.Build();



            // Configure the Http request pipeline

            #region Configure Middleware

            app.MapCarter();
            app.UseExceptionHandler(options => { });

            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            #endregion

            app.Run();
        }
    }
}
