using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressByUserIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task CreateAddressAsync(Address newAddress);
        Task UpdateAddressAsync(Address updatedAddress);
        Task DeleteAddressByUserIdAsync(int id);
    }
}
