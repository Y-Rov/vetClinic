using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioViewModelMapper : IViewModelMapper<Portfolio, PortfolioBaseViewModel>
    {
        public PortfolioBaseViewModel Map(Portfolio source)
        {
            return new PortfolioBaseViewModel
            {
                Description = source.Description
            };
        }
    }
}