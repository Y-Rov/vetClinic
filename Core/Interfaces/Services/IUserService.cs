using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<User> GetUserByIdAsync(int id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task AssignToRoleAsync(User user, string role);
        Task DeleteAsync(User user);
    }
}