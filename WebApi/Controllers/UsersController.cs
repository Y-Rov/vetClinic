using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validators;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IViewModelMapper<User, UserReadViewModel> _readMapper;
        private readonly IViewModelMapper<UserCreateViewModel, User> _createMapper;
        private readonly IViewModelMapperUpdater<UserUpdateViewModel, User> _updateMapper;
        private readonly UserCreateValidator _createValidator;
        private readonly UserValidator<UserUpdateViewModel> _updateValidator;

        public UsersController(IUserService userService,
            IViewModelMapper<User, UserReadViewModel> readMapper,
            IViewModelMapper<UserCreateViewModel, User> createMapper,
            IViewModelMapperUpdater<UserUpdateViewModel, User> updateMapper,
            UserCreateValidator createValidator,
            UserValidator<UserUpdateViewModel> updateValidator)
        {
            _userService = userService;
            _readMapper = readMapper;
            _createMapper = createMapper;
            _updateMapper = updateMapper;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var readViewModels = new List<UserReadViewModel>();

            foreach (var user in users)
            {
                readViewModels.Add(_readMapper.Map(user));
            }

            return Ok(readViewModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadViewModel>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var readViewModel = _readMapper.Map(user!);

            return Ok(readViewModel);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync([FromBody] UserCreateViewModel createDto)
        {
            var validationResult = _createValidator.Validate(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = _createMapper.Map(createDto);

            await _userService.CreateAsync(user, createDto.Password!);
            await _userService.AssignToRoleAsync(user, "Client");

            var readDto = _readMapper.Map(user);

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

            _updateMapper.Map(updateDto, user);
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