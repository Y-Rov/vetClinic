using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        readonly IEmailService _service;

        public EmailController(IEmailService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task SendEmail(EmailMessage email) =>
            await _service.Send(email);

        [HttpPost("/notify")]
        public async Task NotifyUsers(Mailing message) =>
            await _service.NotifyUsers(message);
    }
}
