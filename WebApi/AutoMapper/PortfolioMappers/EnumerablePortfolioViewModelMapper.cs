using Core.Entities;
using Core.ViewModels.PortfolioViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.PortfolioMappers
{
    public class EnumerablePortfolioViewModelMapper : IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioViewModel>>
    {
        private readonly IViewModelMapper<Portfolio, PortfolioViewModel> _readMapper;

        public EnumerablePortfolioViewModelMapper(IViewModelMapper<Portfolio, PortfolioViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<PortfolioViewModel> Map(IEnumerable<Portfolio> source)
        {
            var readViewModels = source.Select(portfolio => _readMapper.Map(portfolio));
            return readViewModels;
        }
    }
}