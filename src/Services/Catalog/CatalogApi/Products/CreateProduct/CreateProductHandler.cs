using BuildingBlocks.CQRS;
using CatalogApi.Models;
using FluentValidation;
using Marten;

namespace CatalogApi.Products.CreateProduct
{

    //***************************

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Is Required !!");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category Is Required !!");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image Is Required !!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price Must be greater than zero !!");
        }
    }

    //***************************

    internal class CreateProductCommandHandler (IDocumentSession session )
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Business Logic To Create a Product 


            // 01 : Crate product entity
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };


            // 02 : Save at the database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);



            // 03 : return a result 
            return  new CreateProductResult(product.Id);

        }
    }



}

