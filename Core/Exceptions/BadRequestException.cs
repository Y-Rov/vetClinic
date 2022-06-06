using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public new string Message { get; set; } = string.Empty;

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

        public BadRequestException(IEnumerable<IdentityError> identityErrors)
        {
            identityErrors
                .ToList()
                .ForEach(error => AddMessage(error));
        }

        private void AddMessage(IdentityError error)
        {
            Message += string.Join(" ", error.Description);
        }
    }
}
