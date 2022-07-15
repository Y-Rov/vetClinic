using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioCreateMapper : IViewModelMapper<PortfolioCreateReadViewModel, Portfolio>
    {
        public Portfolio Map(PortfolioCreateReadViewModel source)
        {
            return new Portfolio
            {
                UserId = source.Id,
                Description = source.Description
            };
        }
    }
}

