namespace CatalogApi.Exceptions
{
    public class NoChangesMadeException(string? message) : Exception(message ?? "There Is No Changes Had Been Added !!")
    {
    }
}
