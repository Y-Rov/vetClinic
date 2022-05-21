using Core.Entities;
using DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private readonly ClinicContext _dataContext;
        public ExceptionController(ClinicContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExceptionEntity>>> GetAll()
        {
            if (_dataContext.Exceptions == null)
            {
                return NotFound();
            }

            return await _dataContext.Exceptions.ToListAsync();
        }
        [HttpGet("Types")]

        public async Task<ActionResult<IEnumerable<object>>> GetTyped()
        {
            if (_dataContext.Exceptions == null)
            {
                return NotFound();
            }
            var query = _dataContext.Exceptions.GroupBy(ex => ex.Name).Select(g => new { Name = g.Key, Count = g.Count() });
            return await query.ToListAsync();
        }
    }
}
