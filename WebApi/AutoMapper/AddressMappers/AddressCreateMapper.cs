using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressCreateMapper : IViewModelMapper<AddressCreateViewModel, Address>
    {
        public Address Map(AddressCreateViewModel source)
        {
            return new Address
            {
                UserId = source.Id,
                City = source.City,
                Street = source.Street,
                House = source.House,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}