using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressUpdateMapper : IViewModelMapperUpdater<AddressBaseViewModel, Address>
    {
        public Address Map(AddressBaseViewModel source)
        {
            return new Address
            {
                City = source.City ?? string.Empty,
                Street = source.Street ?? string.Empty,
                House = source.House ?? string.Empty,
                ZipCode = source.ZipCode ?? string.Empty,
                ApartmentNumber = source.ApartmentNumber ?? 1
            };
        }

        public void Map(AddressBaseViewModel source, Address dest)
        {
            dest.City = source.City ?? dest.City;
            dest.Street = source.Street ?? dest.Street;   
            dest.House = source.House ?? dest.House;
            dest.ApartmentNumber = source.ApartmentNumber ?? dest.ApartmentNumber;
            dest.ZipCode = source.ZipCode ?? dest.ZipCode;
        }
    }
}

