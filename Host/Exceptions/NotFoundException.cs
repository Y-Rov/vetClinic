namespace Host.Exceptions
{
    public class NotFoundException : Exception
    {
        public new string Message { get; set; }
        public NotFoundException()
        {
            Message = "We coudn't find that page";
        }
        public NotFoundException(string message) : base(message)
        {
            Message = message;
        }
        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }
    }
}
