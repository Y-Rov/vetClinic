using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioUpdateMapper : IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio>
    {
        public Portfolio Map(PortfolioBaseViewModel source)
        {
            return new Portfolio
            {
                Description = source.Description
            };
        }

        public void Map(PortfolioBaseViewModel source, Portfolio dest)
        {
            dest.Description = source.Description;
        }
    }
}
