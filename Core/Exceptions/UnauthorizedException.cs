using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public List<IdentityError>? IdentityError { get; set; }
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

        public UnauthorizedException(IEnumerable<IdentityError> identityError)
        {
            Message = "Access is denied due to invalid credentials";
            IdentityError = identityError!.ToList();
        }
    }
}
