using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressByUserIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task CreateAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);
        Task SaveChangesAsync();
    }
}
