using FluentValidation.Results;
using Newtonsoft.Json;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public List<ValidationFailure>? ValidationFailure;
        public new string Message { get; set; }

        public UnauthorizedException()
        {
            Message = "Access is denied due to invalid credentials";
        }

        public UnauthorizedException(string message) : base(message)
        {
            Message = message;
        }

        public UnauthorizedException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }

        public UnauthorizedException(List<ValidationFailure> validationFailures)
        {
            Message = "Illegal request";
            ValidationFailure = validationFailures;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
