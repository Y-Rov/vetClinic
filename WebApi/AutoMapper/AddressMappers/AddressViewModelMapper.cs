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
                Id = source.UserId,
                City = source.City,
                Street = source.Street,
                House = source.House,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}