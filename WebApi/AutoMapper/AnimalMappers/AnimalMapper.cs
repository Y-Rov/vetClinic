using Core.Entities;
using Core.ViewModels.AnimalViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AnimalMappers
{
    public class AnimalMapper : IViewModelMapper<Animal, AnimalViewModel>
    {
        public AnimalViewModel Map(Animal source)
        {
            var animal = new AnimalViewModel
            {
                Id = source.Id,
                OwnerId = source.OwnerId,
                NickName = source.NickName,
                BirthDate = source.BirthDate
            };

            return animal;
        }
    }
}
