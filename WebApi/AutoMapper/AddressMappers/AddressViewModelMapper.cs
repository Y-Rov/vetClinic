using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressViewModelMapper : IViewModelMapper<Address, AddressViewModel>
    {
        public AddressViewModel Map(Address source)
        {
            return new AddressViewModel
            {
                UserId = source.UserId,
                City = source.City,
                ZipCode = source.ZipCode,
                Street = source.Street,
                House = source.House,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}