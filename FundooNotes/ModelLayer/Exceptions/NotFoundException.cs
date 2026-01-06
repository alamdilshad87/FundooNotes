namespace ModelLayer.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
        public override int StatusCode => 404;
    }
}