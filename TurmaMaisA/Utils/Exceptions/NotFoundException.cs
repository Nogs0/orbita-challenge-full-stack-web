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
            : base($"A entidade '{entityName}' com a chave '{key}' não foi encontrada.")
        { }
    }
}
