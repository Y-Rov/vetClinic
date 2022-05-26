using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManager _loggerManager;

        public UserService(
            IUserRepository userRepository, 
            ILoggerManager loggerManager)
        {
            _userRepository = userRepository;
            _loggerManager = loggerManager;
        }

        public async Task AssignToRoleAsync(User user, string role)
        {
            var assignResult = await _userRepository.AssignRoleAsync(user, role);

            if (!assignResult.Succeeded)
            {
                _loggerManager.LogWarn("Failed to assign the role of " +
                    $"{role} to the user with id {user.Id}");
                throw new BadRequestException(assignResult.Errors);
            }

            _loggerManager.LogInfo("Successfully assigned the role of " +
                $"{role} to the user with id {user.Id}");
        }

        public async Task CreateAsync(User user, string password)
        {
            var createResult = await _userRepository.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                _loggerManager.LogWarn("Failed to create a user");
                throw new BadRequestException(createResult.Errors);
            }

            _loggerManager.LogInfo("Successfully created a user");
        }

        public async Task DeleteAsync(User user)
        {
            var deleteResult = await _userRepository.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                _loggerManager.LogWarn($"Failed to delete the user with id {user.Id}");
                throw new BadRequestException(deleteResult.Errors);
            }

            _loggerManager.LogInfo($"Successfully deleted the user with id {user.Id}");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                _loggerManager.LogWarn($"User with id {id} does not exist");
                throw new NotFoundException($"User with id {id} does not exist");
            }

            _loggerManager.LogInfo($"Successfully retrieved the user with id {id}");

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var updateResult = await _userRepository.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _loggerManager.LogWarn($"Failed to update the user with id {user.Id}");
                throw new BadRequestException(updateResult.Errors);
            }

            _loggerManager.LogInfo($"Successfully updated the user with id {user.Id}");
        }
    }
}
