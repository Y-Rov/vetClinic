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
                City = source.City,
                Street = source.Street,
                House = source.House,
                ZipCode = source.ZipCode,
                ApartmentNumber = source.ApartmentNumber
            };
        }

        public void Map(AddressBaseViewModel source, Address dest)
        {
            dest.City = source.City;
            dest.Street = source.Street;   
            dest.House = source.House;
            dest.ZipCode = source.ZipCode;
            dest.ApartmentNumber = source.ApartmentNumber;
        }
    }
}

