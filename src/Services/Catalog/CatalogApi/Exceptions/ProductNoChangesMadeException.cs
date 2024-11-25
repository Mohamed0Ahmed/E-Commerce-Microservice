using BuildingBlocks.Exceptions;

namespace CatalogApi.Exceptions
{

    public class ProductNoChangesMadeException : NoChangesMadeException
    {

        public ProductNoChangesMadeException() : base()
        {

        }
        public ProductNoChangesMadeException(string message) : base(message)
        {

        }
    }
}
