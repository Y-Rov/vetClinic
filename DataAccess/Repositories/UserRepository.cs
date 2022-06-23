using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null)
        {
            var users = await GetQuery(filter, orderBy).ToListAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await GetQuery().SingleOrDefaultAsync(u => u.Id == id);
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
            string role,
            string includeProperties = "")
        {
            var users = (
                from user in _context.Users
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                join r in _context.Roles on userRole.RoleId equals r.Id
                where r.Name == role
                select user
            )
            .Where(u => u.IsActive);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                users = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(users, (current, includeProperty) => current.Include(includeProperty));
            }

            return await users.ToListAsync();
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
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null)
        {
            IQueryable<User> usersQuery = (
                filter is null 
                ? _userManager.Users 
                : _userManager.Users.Where(filter)
            )
            .Where(u => u.IsActive)
            .Include(u => u.Address)
            .Include(u => u.Portfolio);

            if (orderBy is not null)
            {
                usersQuery = orderBy(usersQuery);
            }

            return usersQuery;
        }
    }
}
