using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioMapper : IViewModelMapper<PortfolioViewModel, Portfolio>
    {
        public Portfolio Map(PortfolioViewModel source)
        {
            return new Portfolio
            {
                UserId = source.UserId,
                Description = source.Description
            };
        }
    }
}

