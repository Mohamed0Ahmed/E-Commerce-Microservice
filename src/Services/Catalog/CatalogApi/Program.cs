using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using CatalogApi.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace CatalogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Services to the Container 

            #region configure Services

            // Configure MediatR
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


            builder.Services.AddCarter();

            // Configure Marten 
            builder.Services.AddMarten(option =>
            {
                option.Connection(builder.Configuration.GetConnectionString("Database")!);
            }).UseLightweightSessions();

            if (builder.Environment.IsDevelopment())
                builder.Services.InitializeMartenWith<CatalogInitialData>();


            builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

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
