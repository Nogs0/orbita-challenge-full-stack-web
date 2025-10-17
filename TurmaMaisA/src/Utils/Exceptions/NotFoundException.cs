namespace TurmaMaisA.Utils.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() 
            : base() 
        { }

        public NotFoundException(string message) 
            : base(message) 
        { }

        public NotFoundException(string entityName, object key) 
            : base($"The entity '{entityName}' with key '{key}' was not found.")
        { }
    }
}
