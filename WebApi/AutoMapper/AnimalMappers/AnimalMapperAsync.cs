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
    public class AnimalMapperAsync : IViewModelMapperAsync<Animal, AnimalViewModel>
    {
        public async Task<AnimalViewModel> MapAsync(Animal source)
        {
            var animalViewModel =  new AnimalViewModel
            {
                Id = source.Id,
                OwnerId = source.OwnerId,
                NickName = source.NickName,
                BirthDate = source.BirthDate,
            };
            return animalViewModel;
        }
    }
}
