using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioCreateReadViewModelMapper : IViewModelMapper<Portfolio, PortfolioCreateReadViewModel>
    {
        public PortfolioCreateReadViewModel Map(Portfolio source)
        {
            return new PortfolioCreateReadViewModel
            {
                Id = source.UserId,
                Description = source.Description
            };
        }
    }
}
