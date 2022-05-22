using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task SaveChangesAsync();
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
