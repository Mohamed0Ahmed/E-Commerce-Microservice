﻿using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool IsSuccess);


    //*****************************
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteBasketCommand(userName));

                    var response = result.Adapt<DeleteBasketResponse>();

                    return Results.Ok(response);
                })
                  .WithName("DeleteProductFromBasket")
                  .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                  .ProducesProblem(StatusCodes.Status400BadRequest)
                  .ProducesProblem(StatusCodes.Status404NotFound)
                  .WithSummary("Delete Product From Basket")
                  .WithDescription("Delete Product From Basket");

        }
    }

}
