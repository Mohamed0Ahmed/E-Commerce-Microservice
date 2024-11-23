using BuildingBlocks.CQRS;
using CatalogApi.Exceptions;
using CatalogApi.Models;
using Marten;

namespace CatalogApi.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) :ICommand<DeleteProductResult>;
    public record DeleteProductResult (bool IsSuccess);


    //***************************************

    internal class DeleteProductCommandHandler(IDocumentSession session , ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            logger.LogInformation("Attempting to Delete product with Id: {Id}", command.Id);

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException("No Product To Delete With This Id");
            }
            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);


            return new DeleteProductResult(true);
        }
    }


}
