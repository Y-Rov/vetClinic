using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<IdentityError>? IdentityError { get; set; }
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

        public BadRequestException(IEnumerable<IdentityError> identityError)
        {
            Message = "Illegal request";
            IdentityError = identityError.ToList();
        }
    }
}
