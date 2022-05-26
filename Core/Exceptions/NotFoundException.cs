using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public new string Message { get; set; } = string.Empty;

        public NotFoundException()
        {
            Message = "We coudn't find that";
        }

        public NotFoundException(string message) : base(message)
        {
            Message = message;
        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }

        public NotFoundException(IEnumerable<IdentityError> identityErrors)
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
