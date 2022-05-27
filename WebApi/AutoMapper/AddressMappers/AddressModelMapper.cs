using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressModelMapper : IViewModelMapper<AddressViewModel, Address>
    {
        public Address Map(AddressViewModel source)
        {
            return new Address
            {
                UserId = source.UserId,
                City = source.City,
                Street = source.Street,
                House = source.House,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}