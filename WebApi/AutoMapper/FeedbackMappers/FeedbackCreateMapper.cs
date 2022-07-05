using Core.Entities;
using Core.ViewModels.FeedbackViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.FeedbackMappers
{
    public class FeedbackCreateMapper : IViewModelMapper<FeedbackCreateViewModel, Feedback>
    {
        public Feedback Map(FeedbackCreateViewModel source)
        {
            return new Feedback
            {
                Email = source.Email,
                ServiceRate = source.ServiceRate,
                PriceRate = source.PriceRate,
                SupportRate = source.SupportRate,
                Suggestions = source.Suggestions,
                UserId = source.UserId
            };
        }
    }
}
