using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressViewModelMapper : IViewModelMapper<Address, AddressBaseViewModel>
    {
        public AddressBaseViewModel Map(Address source)
        {
            return new AddressBaseViewModel
            {
                City = source.City,
                Street = source.Street,
                House = source.House,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}