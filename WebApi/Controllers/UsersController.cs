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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IViewModelMapper<User, UserReadViewModel> _readMapper;
        private readonly IViewModelMapper<UserCreateViewModel, User> _createMapper;
        private readonly IViewModelMapperUpdater<UserUpdateViewModel, User> _updateMapper;

        public UsersController(
            IUserService userService,
            IViewModelMapper<User, UserReadViewModel> readMapper,
            IViewModelMapper<UserCreateViewModel, User> createMapper,
            IViewModelMapperUpdater<UserUpdateViewModel, User> updateMapper)
        {
            _userService = userService;
            _readMapper = readMapper;
            _createMapper = createMapper;
            _updateMapper = updateMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var readModels = new List<UserReadViewModel>();

            foreach (var user in users)
            {
                readModels.Add(_readMapper.Map(user));
            }

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<UserReadViewModel>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var readModel = _readMapper.Map(user!);

            return Ok(readModel);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync([FromBody] UserCreateViewModel createModel)
        {
            var user = _createMapper.Map(createModel);
            await _userService.CreateAsync(user, createModel.Password!);
            await _userService.AssignToRoleAsync(user, "Client");

            var readModel = _readMapper.Map(user);

            return CreatedAtAction(nameof(GetAsync), new { id = readModel.Id }, readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, 
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

        [Authorize(Roles = "Admin")]
        [HttpPost("register/{role}")]
        public async Task<ActionResult<UserReadViewModel>> CreateAsync([FromRoute] string role, 
            [FromBody] UserCreateViewModel createModel)
        {
            var user = _createMapper.Map(createModel);
            await _userService.CreateAsync(user, createModel.Password!);
            await _userService.AssignToRoleAsync(user, role);

            var readDto = _readMapper.Map(user);

            return CreatedAtAction(nameof(GetAsync), new { id = readDto.Id }, readDto);
        }
    }
}