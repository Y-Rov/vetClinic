using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
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

        private IQueryable<User> GetQuery(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null)
        {
            IQueryable<User> usersQuery = (filter is null ? 
                _userManager.Users : 
                _userManager.Users.Where(filter))
                .Where(u => u.IsActive)
                .Include(u => u.Address)
                .Include(u => u.Portfolio)
                .Include(u => u.UserSpecializations);

            if (orderBy is not null)
            {
                usersQuery = orderBy(usersQuery);
            }

            return usersQuery;
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
    }
}
