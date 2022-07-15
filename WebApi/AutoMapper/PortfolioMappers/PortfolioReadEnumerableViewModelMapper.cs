using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioReadEnumerableViewModelMapper 
        : IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>>
    {
        private readonly IViewModelMapper<Portfolio, PortfolioCreateReadViewModel> _readMapper;

        public PortfolioReadEnumerableViewModelMapper(IViewModelMapper<Portfolio, PortfolioCreateReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<PortfolioCreateReadViewModel> Map(IEnumerable<Portfolio> source)
        {
            var readViewModels = source.Select(portfolio => _readMapper.Map(portfolio));
            return readViewModels;
        }
    }
}