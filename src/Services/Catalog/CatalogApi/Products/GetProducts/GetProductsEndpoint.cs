using Carter;
using CatalogApi.Models;
using Mapster;
using MediatR;

namespace CatalogApi.Products.GetProducts
{

    public record GetProductsResponse(IEnumerable<Product> Products);

    //**************************
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());

                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
                .WithName("GetProduct")
                .Produces<GetProductsResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product")
                .WithDescription("Get Product"); ;
        }
    }
}
