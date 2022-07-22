using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        readonly IEmailService service;

        public EmailController(IEmailService service)
        {
            this.service = service;
        }
    }
}
