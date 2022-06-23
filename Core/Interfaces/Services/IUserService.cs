using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetDoctorsAsync(string specialization = "");
        Task<User> GetUserByIdAsync(int id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task AssignRoleAsync(User user, string role);
        Task DeleteAsync(User user);
    }
}