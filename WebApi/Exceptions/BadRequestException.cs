using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Exceptions
{
    public class BadRequestException : Exception
    {
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
    }
}
