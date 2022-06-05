using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AssignRoleAsync(User user, string role);
        Task<IdentityResult> UpdateAsync(User user);
        void Delete(User user);
    }
}