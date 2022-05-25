using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AssignToRoleAsync(User user, string role)
        {
            var assignResult = await _userRepository.AssignRoleAsync(user, role);

            if (!assignResult.Succeeded)
            {
                throw new BadRequestException(assignResult.Errors);
            }
        }

        public async Task CreateAsync(User user, string password)
        {
            var createResult = await _userRepository.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new BadRequestException();
            }
        }

        public async Task DeleteAsync(User user)
        {
            var deleteResult = await _userRepository.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                throw new BadRequestException();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                throw new NotFoundException($"User with id {id} not found");
            }

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var updateResult = await _userRepository.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new BadRequestException();
            }
        }
    }
}