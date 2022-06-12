using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressByUserIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task CreateAddressAsync(Address address);
        Task UpdateAddressAsync(Address updatedAddress);
        Task DeleteAddressByUserIdAsync(int id);
    }
}
