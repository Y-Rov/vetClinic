using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioCreateMapper : IViewModelMapper<PortfolioCreateViewModel, Portfolio>
    {
        public Portfolio Map(PortfolioCreateViewModel source)
        {
            return new Portfolio
            {
                UserId = source.Id,
                Description = source.Description
            };
        }
    }
}

