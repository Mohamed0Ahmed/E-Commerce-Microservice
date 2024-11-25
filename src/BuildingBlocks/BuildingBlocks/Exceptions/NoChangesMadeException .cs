namespace BuildingBlocks.Exceptions
{
    public class NoChangesMadeException : Exception
    {
        public NoChangesMadeException()
            : base("No changes were made to the existing data !!")
        {
        }

        public NoChangesMadeException(string message)
            : base(message)
        {
        }

       
    }

}
