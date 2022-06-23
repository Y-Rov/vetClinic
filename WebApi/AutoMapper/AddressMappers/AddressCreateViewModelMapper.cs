using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressCreateViewModelMapper : IViewModelMapper<Address, AddressCreateViewModel>
    {
        public AddressCreateViewModel Map(Address source)
        {
            return new AddressCreateViewModel
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
