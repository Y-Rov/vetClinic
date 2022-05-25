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
