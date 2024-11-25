using BuildingBlocks.CQRS;
using CatalogApi.Exceptions;
using CatalogApi.Models;
using CatalogApi.Products.CreateProduct;
using FluentValidation;
using Marten;

namespace CatalogApi.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id Is Required !!");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name Is Required !!")
                    .Length(5, 25).WithMessage("Name must be between 5 and 25 chars ");
            RuleFor(command => command.Category).NotEmpty().WithMessage("Category Is Required !!");
            RuleFor(command => command.ImageFile).NotEmpty().WithMessage("Image Is Required !!");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price Must be greater than zero !!");
        }
    }



    //************************************


    internal class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {

        #region Helper Method
        private void UpdateProductDetails(Product product, UpdateProductCommand command)
        {
            product.Name = string.IsNullOrEmpty(command.Name) ? product.Name : command.Name;
            product.Category = command.Category?.Any() ?? false ? command.Category : product.Category;
            product.Description = string.IsNullOrEmpty(command.Description) ? product.Description : command.Description;
            product.Price = command.Price != 0 ? command.Price : product.Price;
            product.ImageFile = string.IsNullOrEmpty(command.ImageFile) ? product.ImageFile : command.ImageFile;
        }

        #endregion


        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            //logger.LogInformation("Attempting to update product with Id: {Id}, Name: {Name}", command.Id, command.Name); // add to loggingBehavior

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            if (product.Name == command.Name && product.Category.SequenceEqual(command.Category) &&
                product.Description == command.Description && product.Price == command.Price &&
                product.ImageFile == command.ImageFile)

                throw new ProductNoChangesMadeException();



            UpdateProductDetails(product, command);
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true); // Successfully updated
        }
    }
}
