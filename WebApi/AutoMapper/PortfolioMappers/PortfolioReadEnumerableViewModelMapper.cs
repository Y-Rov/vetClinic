using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class PortfolioReadEnumerableViewModelMapper 
        : IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateViewModel>>
    {
        private readonly IViewModelMapper<Portfolio, PortfolioCreateViewModel> _readMapper;

        public PortfolioReadEnumerableViewModelMapper(IViewModelMapper<Portfolio, PortfolioCreateViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<PortfolioCreateViewModel> Map(IEnumerable<Portfolio> source)
        {
            var readViewModels = source.Select(portfolio => _readMapper.Map(portfolio));
            return readViewModels;
        }
    }
}