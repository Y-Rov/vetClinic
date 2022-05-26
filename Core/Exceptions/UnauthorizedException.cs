using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public new string Message { get; set; } = string.Empty;

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

        public UnauthorizedException(IEnumerable<IdentityError> identityErrors)
        {
            identityErrors
                .ToList()
                .ForEach(error => AddMessage(error));
        }

        private void AddMessage(IdentityError error)
        {
            Message += error.Description.Concat(" ");
        }
    }
}
