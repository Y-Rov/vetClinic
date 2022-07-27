using Core.Entities;
using Core.ViewModels.AddressViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AddressMappers
{
    public class AddressUpdateMapper : IViewModelMapperUpdater<AddressUpdateViewModel, Address>
    {
        public Address Map(AddressUpdateViewModel source)
        {
            return new Address
            {
                City = source.City ?? string.Empty,
                Street = source.Street ?? string.Empty,
                House = source.House ?? string.Empty,
                ApartmentNumber = source.ApartmentNumber,
                ZipCode = source.ZipCode
            };
        }

        public void Map(AddressUpdateViewModel source, Address dest)
        {
            dest.City = source.City ?? dest.City;
            dest.Street = source.Street ?? dest.Street;   
            dest.House = source.House ?? dest.House;
            dest.ApartmentNumber = source.ApartmentNumber;
            dest.ZipCode = source.ZipCode;
        }
    }
}

