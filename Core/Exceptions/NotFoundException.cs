using FluentValidation.Results;

namespace Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public List<ValidationFailure>? ValidationFailure;
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

        public NotFoundException(List<ValidationFailure> validationFailures)
        {
            Message = "Illegal request";
            ValidationFailure = validationFailures;
        }
    }
}
