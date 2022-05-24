using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class ForbidException : Exception
    {
        public List<IdentityError>? IdentityError { get; set; }
        public new string Message { get; set; }

        public ForbidException()
        {
            Message = "You don't have permission to access / on this server";
        }

        public ForbidException(string message) : base(message)
        {
            Message = message;
        }

        public ForbidException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }

        public ForbidException(IEnumerable<IdentityError>? identityError)
        {
            Message = "You don't have permission to access / on this server";
            IdentityError = identityError!.ToList();
        }
    }
}
