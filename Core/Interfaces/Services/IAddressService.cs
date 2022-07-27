using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressByUserIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task CreateAddressAsync(Address newAddress);
        Task UpdateAddressAsync(Address oldAddress);
        Task DeleteAddressByUserIdAsync(int id);
    }
}
