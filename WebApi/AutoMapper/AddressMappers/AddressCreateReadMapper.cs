using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressCreateReadMapper : IViewModelMapper<AddressCreateReadViewModel, Address>
    {
        public Address Map(AddressCreateReadViewModel source)
        {
            return new Address
            {
                UserId = source.Id,
                City = source.City!,
                Street = source.Street!,
                House = source.House!,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }
    }
}