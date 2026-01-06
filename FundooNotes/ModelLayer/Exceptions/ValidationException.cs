namespace ModelLayer.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
        public override int StatusCode => 400;
    }
}