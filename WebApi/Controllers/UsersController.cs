using AutoMapper;
using Core.ViewModel;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validators;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserCreateDtoValidator _createValidator;
        private readonly UserUpdateDtoValidator _updateValidator;

        public UsersController(IUserService userService, IMapper mapper,
            UserCreateDtoValidator createValidator, 
            UserUpdateDtoValidator updateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            var readDtos = _mapper.Map<IEnumerable<UserReadViewModel>>(users);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadViewModel>> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var readDto = _mapper.Map<UserReadViewModel>(user);

            return Ok(readDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadViewModel>> Create(UserCreateViewModel createDto)
        {
            var validationResult = _createValidator.Validate(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = _mapper.Map<User>(createDto);
            var createResult = await _userService.CreateAsync(user, createDto.Password!);

            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }

            var readDto = _mapper.Map<UserReadViewModel>(user);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UserUpdateViewModel updateDto)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var validationResult = _updateValidator.Validate(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _mapper.Map(updateDto, user);
            var updateResult = await _userService.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var deleteResult = await _userService.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                return BadRequest(deleteResult.Errors);
            }

            return NoContent();
        }
    }
}