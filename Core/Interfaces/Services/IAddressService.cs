using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task CreateAddressAsync(Address portfolio);
        Task<Address> UpdateAddressAsync(Address portfolio);
        Task DeleteAddressAsync(int id);
    }
}
