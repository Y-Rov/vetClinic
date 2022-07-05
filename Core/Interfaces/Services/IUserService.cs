using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters);
        Task<IEnumerable<User>> GetDoctorsAsync(string specialization = "");
        Task<User> GetUserByIdAsync(int id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task AssignRoleAsync(User user, string role);
        Task DeleteAsync(User user);
    }
}