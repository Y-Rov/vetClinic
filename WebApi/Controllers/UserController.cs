using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IViewModelMapper<User, UserReadViewModel> _readMapper;
        private readonly IViewModelMapper<UserCreateViewModel, User> _createMapper;
        private readonly IViewModelMapperUpdater<UserUpdateViewModel, User> _updateMapper;
        private readonly IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> _readEnumerableMapper;

        public UserController(
            IUserService userService,
            IViewModelMapper<User, UserReadViewModel> readMapper,
            IViewModelMapper<UserCreateViewModel, User> createMapper,
            IViewModelMapperUpdater<UserUpdateViewModel, User> updateMapper,
            IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> readEnumerableMapper)
        {
            _userService = userService;
            _readMapper = readMapper;
            _createMapper = createMapper;
            _updateMapper = updateMapper;
            _readEnumerableMapper = readEnumerableMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetAsync(
            [FromQuery(Name = "takeCount")] int? takeCount,
            [FromQuery(Name = "skipCount")] int skipCount = 0)
        {
            var users = await _userService.GetAllUsersAsync(takeCount, skipCount);
            var readModels = _readEnumerableMapper.Map(users);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadViewModel>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var readModel = _readMapper.Map(user!);

            return Ok(readModel);
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetDoctorsAsync(
            [FromQuery(Name = "specialization")] string? specialization)
        {
            var users = await _userService.GetDoctorsAsync(specialization!);
            var readModels = _readEnumerableMapper.Map(users);

            return Ok(readModels);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync([FromBody] UserCreateViewModel createModel)
        {
            var user = _createMapper.Map(createModel);
            await _userService.CreateAsync(user, createModel.Password!);
            await _userService.AssignRoleAsync(user, "Client");

            var readModel = _readMapper.Map(user);

            return CreatedAtAction(nameof(GetAsync), new { id = readModel.Id }, readModel);
        }

        [HttpPost("register/{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync(
            [FromRoute] string role,
            [FromBody] UserCreateViewModel createModel)
        {
            var user = _createMapper.Map(createModel);
            await _userService.CreateAsync(user, createModel.Password!);
            await _userService.AssignRoleAsync(user, role);

            var readDto = _readMapper.Map(user);

            return CreatedAtAction(nameof(GetAsync), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync(
            [FromRoute] int id, 
            [FromBody] UserUpdateViewModel updateModel)
        {
            var user = await _userService.GetUserByIdAsync(id);

            _updateMapper.Map(updateModel, user);
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