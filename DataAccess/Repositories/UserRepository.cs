using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ClinicContext _context;

        public UserRepository(
            UserManager<User> userManager,
            ClinicContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var createResult = await _userManager.CreateAsync(user, password);
            return createResult;
        }

        public void Delete(User user)
        {
            user.IsActive = false;
        }

        public async Task<PagedList<User>> GetAllAsync(
            UserParameters userParameters,
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null)
        {
            var users = await GetQuery(filter, orderBy, includeProperties)
                .ToPagedListAsync(userParameters.PageNumber, userParameters.PageSize);

            return users;
        }

        public async Task<User?> GetByIdAsync(
            int id, 
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null)
        {
            var user = await GetQuery(includeProperties: includeProperties)
                .SingleOrDefaultAsync(u => u.Id == id);

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

        public async Task<IEnumerable<User>> GetByRolesAsync(
            List<int> roleIds,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null)
        {
            var roles = _context.Roles.Where(r => roleIds.Contains(r.Id));

            var usersQuery = GetQuery(
                u => u.Id == _context.UserRoles
                    .SingleOrDefault(ur => ur.RoleId == roles.SingleOrDefault(r => r.Id == ur.RoleId)!.Id && ur.UserId == u.Id)!.UserId,
                orderBy,
                includeProperties);

            var users = await usersQuery.ToListAsync();

            return users;
        }

        public IEnumerable<User> FilterBySpecialization(IEnumerable<User> users, string specialization)
        {
            var filteredUsers = users.Where(
                u => u.UserSpecializations.Any(
                    us => us.Specialization?.Name
                        .ToLower()
                        .Contains(specialization.Trim().ToLower())
                        ?? false));

            return filteredUsers;
        }

        private IQueryable<User> GetQuery(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null)
        {
            IQueryable<User> usersQuery = (
                filter is null
                ? _userManager.Users
                : _userManager.Users.Where(filter)
            )
            .Where(u => u.IsActive);

            if (includeProperties is not null)
            {
                usersQuery = includeProperties(usersQuery);
            }

            if (orderBy is not null)
            {
                usersQuery = orderBy(usersQuery);
            }

            return usersQuery;
        }
    }
}
