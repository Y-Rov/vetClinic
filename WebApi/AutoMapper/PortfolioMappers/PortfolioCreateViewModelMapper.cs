using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioCreateViewModelMapper : IViewModelMapper<Portfolio, PortfolioCreateViewModel>
    {
        public PortfolioCreateViewModel Map(Portfolio source)
        {
            return new PortfolioCreateViewModel
            {
                Id = source.UserId,
                Description = source.Description
            };
        }
    }
}
