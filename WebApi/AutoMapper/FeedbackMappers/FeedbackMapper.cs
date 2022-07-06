using Core.Entities;
using Core.ViewModels.FeedbackViewModels;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.FeedbackMappers
{
    public class FeedbackMapper : IViewModelMapper<Feedback, FeedbackReadViewModel>
    {
        readonly IViewModelMapper<User, UserReadViewModel> _userMapper;

        public FeedbackMapper(IViewModelMapper<User, UserReadViewModel> userMapper)
        {
            _userMapper = userMapper;
        }

        public FeedbackReadViewModel Map(Feedback source)
        {
            return new FeedbackReadViewModel
            {
                Email = source.Email,
                ServiceRate = source.ServiceRate,
                PriceRate = source.PriceRate,
                SupportRate = source.SupportRate,
                Suggestions = source.Suggestions,
                User = _userMapper.Map(source.User)
            };
        }
    }
}
