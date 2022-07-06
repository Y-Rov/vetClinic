using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetAllAsync(
            UserParameters userParameters,
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null);

        Task<IEnumerable<User>> GetByRolesAsync(
            List<string> roleNames,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null);

        Task<User?> GetByIdAsync(
            int id,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null);

        IEnumerable<User> FilterBySpecialization(IEnumerable<User> users, string specialization);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AssignRoleAsync(User user, string role);
        Task<IdentityResult> UpdateAsync(User user);
        void Delete(User user);
    }
}