using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validators;
using Core.ViewModels.User;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserCreateValidator _createValidator;
        private readonly UserValidator<UserUpdateViewModel> _updateValidator;

        public UsersController(IUserService userService, IMapper mapper,
            UserCreateValidator createValidator, 
            UserValidator<UserUpdateViewModel> updateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var readDtos = _mapper.Map<IEnumerable<UserReadViewModel>>(users);

            return Ok(readDtos);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadViewModel>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var readDto = _mapper.Map<UserReadViewModel>(user);

            return Ok(readDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync([FromBody] UserCreateViewModel createDto)
        {
            var validationResult = _createValidator.Validate(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = _mapper.Map<User>(createDto);

            await _userService.CreateAsync(user, createDto.Password!);

            var readDto = _mapper.Map<UserReadViewModel>(user);

            return CreatedAtAction(nameof(GetAsync), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UserUpdateViewModel updateDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var validationResult = _updateValidator.Validate(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _mapper.Map(updateDto, user);
             await _userService.UpdateAsync(user!);

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            await _userService.DeleteAsync(user!);

            return NoContent();
        }
    }
}