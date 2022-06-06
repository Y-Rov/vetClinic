using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioViewModelMapper : IViewModelMapper<Portfolio, PortfolioViewModel>
    {
        public PortfolioViewModel Map(Portfolio source)
        {
            return new PortfolioViewModel
            {
                UserId = source.UserId,
                Description = source.Description
            };
        }
    }
}