using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<User>> GetByRoleAsync(
            string roleName,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "");

        IEnumerable<User> FilterBySpecialization(IEnumerable<User> users, string specialization);
        Task<User?> GetByIdAsync(int id, string includeProperties = "");
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AssignRoleAsync(User user, string role);
        Task<IdentityResult> UpdateAsync(User user);
        void Delete(User user);
    }
}