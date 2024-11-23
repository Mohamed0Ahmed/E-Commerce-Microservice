using Carter;
using Mapster;
using MediatR;

namespace CatalogApi.Products.CreateProduct
{

    public record CreateProductsRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public record CreateProductsResponse(Guid Id);


    //***********************************

    public class CreateProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapPost("/products", async (CreateProductsRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductsResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
                .WithName("CreateProduct")
                .Produces<CreateProductsResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
 