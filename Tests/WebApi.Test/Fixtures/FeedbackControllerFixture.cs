using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.FeedbackViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class FeedbackControllerFixture
    {
        public FeedbackControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            
        }

        public FeedbackController MockController { get; }
        public Mock<IFeedbackService> MockFeedbackService { get; }
        public Mock<IViewModelMapper<FeedbackCreateViewModel, Feedback>> MockCreateFeedbackMapper { get; }
        public Mock<IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>>> MockListFeedbackMapper { get; }
    }
}
