using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<User?> GetByIdAsync(int id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AssignRoleAsync(User user, string role);
        Task<IdentityResult> UpdateAsync(User user);
        void Delete(User user);
    }
}