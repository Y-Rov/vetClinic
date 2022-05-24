using FluentValidation.Results;

namespace Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<ValidationFailure>? ValidationFailure;
        public new string Message { get; set; }

        public BadRequestException()
        {
            Message = "Illegal request";
        }

        public BadRequestException(string message) : base(message)
        {
            Message = message;
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }

        public BadRequestException(List<ValidationFailure> validationFailures)
        {
            Message = "Illegal request";
            ValidationFailure = validationFailures;
        }
    }
}
