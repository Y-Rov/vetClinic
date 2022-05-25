using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task AssignToRoleAsync(User user, string role);
    }
}