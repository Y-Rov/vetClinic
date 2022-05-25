using Microsoft.AspNetCore.Identity;

namespace Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public List<IdentityError>? IdentityError { get; set; }
        public new string Message { get; set; }
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

        public NotFoundException(IEnumerable<IdentityError>? identityError)
        {
            Message = "We coudn't find that";
            IdentityError = identityError!.ToList();
        }
    }
}
