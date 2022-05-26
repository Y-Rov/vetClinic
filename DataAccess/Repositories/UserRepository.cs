using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var createResult = await _userManager.CreateAsync(user, password);
            return createResult;
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            var deleteResult = await _userManager.DeleteAsync(user);
            return deleteResult;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult;
        }

        public async Task<IdentityResult> AssignRoleAsync(User user, string role)
        {
            var assignResult = await _userManager.AddToRoleAsync(user, role);
            return assignResult;
        }
    }
}
