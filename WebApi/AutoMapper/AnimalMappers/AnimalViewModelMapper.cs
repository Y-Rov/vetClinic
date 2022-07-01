using Core.Entities;
using Core.ViewModels.AnimalViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AnimalMappers
{
    public class AnimalViewModelMapper : IViewModelMapper<AnimalViewModel, Animal>
    {
        public Animal Map(AnimalViewModel source)
        {
            var animalViewModel = new Animal
            {
                Id = source.Id,
                OwnerId = source.OwnerId,
                NickName = source.NickName,
                BirthDate = source.BirthDate,
                PhotoUrl = source.PhotoUrl
            };

            return animalViewModel;
        }
    }
}
