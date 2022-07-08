using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AnimalMappers
{
    public class AnimalPagedMedCardMapper : IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>
    {
        private readonly IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>> _medCardModel;

        public AnimalPagedMedCardMapper(IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>> medCardModel)
        {
            _medCardModel = medCardModel;
        }

        public PagedReadViewModel<AnimalMedCardViewModel> Map(PagedList<Appointment> source)
        {
            return new PagedReadViewModel<AnimalMedCardViewModel>()
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                Entities = _medCardModel.Map(source.AsEnumerable())
            };
        }
    }
}
