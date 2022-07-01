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
    public class AnimalListToListMapper : IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>
    {
        public IEnumerable<AnimalViewModel> Map(IEnumerable<Animal> source)
        {
            var appointmentViewModels = source.Select(GetAppointmentViewModel).ToList();
            return appointmentViewModels;
        }

        private AnimalViewModel GetAppointmentViewModel(Animal animal)
        {
            var animalViewModel = new AnimalViewModel()
            {
                Id = animal.Id,
                BirthDate = animal.BirthDate,
                NickName = animal.NickName,
                OwnerId = animal.OwnerId,
                PhotoUrl = animal.PhotoUrl,
            };

            return animalViewModel;
        }
    }
}
