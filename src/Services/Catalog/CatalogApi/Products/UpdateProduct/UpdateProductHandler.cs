using BuildingBlocks.CQRS;
using CatalogApi.Exceptions;
using CatalogApi.Models;
using Marten;

namespace CatalogApi.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        private void UpdateProductDetails(Product product, UpdateProductCommand command)
        {
            product.Name = string.IsNullOrEmpty(command.Name) ? product.Name : command.Name;
            product.Category = command.Category?.Any() ?? false ? command.Category : product.Category;
            product.Description = string.IsNullOrEmpty(command.Description) ? product.Description : command.Description;
            product.Price = command.Price != 0 ? command.Price : product.Price;
            product.ImageFile = string.IsNullOrEmpty(command.ImageFile) ? product.ImageFile : command.ImageFile;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Attempting to update product with Id: {Id}, Name: {Name}", command.Id, command.Name);

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException("No Product To Update With This Id");
            }

            if (product.Name == command.Name && product.Category.SequenceEqual(command.Category) &&
                product.Description == command.Description && product.Price == command.Price &&
                product.ImageFile == command.ImageFile)
            {
                //return new UpdateProductResult(false); // No changes detected
                throw new NoChangesMadeException("No changes Has Made !!");

            }

            UpdateProductDetails(product, command);
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true); // Successfully updated
        }
    }
}
