namespace CatalogApi.Exceptions
{
    public class ProductNotFoundException(string? message) : Exception(message?? "Product Is Not Found  !!")
    {
    }

}
