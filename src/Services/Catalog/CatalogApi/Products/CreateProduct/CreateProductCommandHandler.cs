using BuildingBlocks.CQRS;
using CatalogApi.Models;
using MediatR;

namespace CatalogApi.Products.CreateProduct
{

    //***************************

    public record CreateProductCommand(string name, List<string> category, string description, string imageFile, decimal Price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid id);

    //***************************

    internal class CreateProductCommandHandler 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Bussines Logic To Create a Product 


            // 01 : Crate product entity
            var product = new Product
            {
                Name = command.name,
                Category = command.category,
                Description = command.description,
                ImageFile = command.imageFile,
                Price = command.Price
            };


            // 02 : Save at the database // Todo


            // 03 : returen a result 

            return  new CreateProductResult(Guid.NewGuid());

        }
    }



}

