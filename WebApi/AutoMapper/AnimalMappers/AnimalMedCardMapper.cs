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
    public class AnimalMedCardMapper : IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>>
    {
        public IEnumerable<AnimalMedCardViewModel> Map(IEnumerable<Appointment> source)
        {
            var appointmentViewModels = source.Select(GetAnimalMedCardViewModel).ToList();
            return appointmentViewModels;
        }

        private AnimalMedCardViewModel GetAnimalMedCardViewModel(Appointment appointment)
        {
            var animalViewModel = new AnimalMedCardViewModel()
            {
                Id = appointment.Id,
                AnimalId = appointment.AnimalId,
                Date = appointment.Date,
                Disease = appointment.Disease,
                MeetHasOccureding = appointment.MeetHasOccureding
            };

            return animalViewModel;
        }
    }
}
