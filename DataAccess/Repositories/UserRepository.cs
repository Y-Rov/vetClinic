using Core.Entities;
using Core.Interfaces.Repositories;
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

        public async Task<IEnumerable<User>> GetAllAsync(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null,
            int? takeCount = null,
            int skipCount = 0)
        {
            var users = await GetQuery(filter, orderBy, includeProperties, takeCount, skipCount)
                .ToListAsync();

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

        public async Task<IEnumerable<User>> GetByRoleAsync(
            string roleName,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null,
            int? takeCount = null,
            int skipCount = 0)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            var usersQuery = GetQuery(
                u => u.Id == _context.UserRoles
                    .SingleOrDefault(ur => ur.RoleId == role!.Id && ur.UserId == u.Id)!.UserId,
                orderBy,
                includeProperties,
                takeCount,
                skipCount);

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
            Func<IQueryable<User>, IIncludableQueryable<User, object>>? includeProperties = null,
            int? takeCount = null,
            int skipCount = 0)
        {
            IQueryable<User> usersQuery = (
                filter is null
                ? _userManager.Users
                : _userManager.Users.Where(filter)
            )
            .Where(u => u.IsActive)
            .Skip(skipCount);

            if (includeProperties is not null)
            {
                usersQuery = includeProperties(usersQuery);
            }

            if (orderBy is not null)
            {
                usersQuery = orderBy(usersQuery);
            }

            if (takeCount is not null)
            {
                usersQuery = usersQuery.Take(takeCount.Value);
            }

            return usersQuery;
        }
    }
}
