using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
    }
}