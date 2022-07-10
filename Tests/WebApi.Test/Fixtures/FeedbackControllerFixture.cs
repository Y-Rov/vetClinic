using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
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

            ExpectedFeedbacks = GenerateFeedbacks();
            ExpectedFeedbacksViewModel = GenerateFeedbacksViewModel();
            TestFeedbackCreateViewModel = GenerateCreateFeedbackViewModel();
            TestParameters = GenerateParameters();
            TestFeedback = GenerateFeedback();

            MockFeedbackService = fixture.Freeze<Mock<IFeedbackService>>();
            MockCreateFeedbackMapper = fixture.Freeze<Mock<IViewModelMapper<FeedbackCreateViewModel, Feedback>>>();
            MockListFeedbackMapper = fixture.Freeze<Mock<IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>>>>();

            MockController = new FeedbackController(
                MockFeedbackService.Object,
                MockCreateFeedbackMapper.Object,
                MockListFeedbackMapper.Object);
        }

        public FeedbackController MockController { get; }
        public Mock<IFeedbackService> MockFeedbackService { get; }
        public Mock<IViewModelMapper<FeedbackCreateViewModel, Feedback>> MockCreateFeedbackMapper { get; }
        public Mock<IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>>> MockListFeedbackMapper { get; }

        public IEnumerable<Feedback> ExpectedFeedbacks { get; set; }
        public IEnumerable<FeedbackReadViewModel> ExpectedFeedbacksViewModel { get; set; }
        public FeedbackCreateViewModel TestFeedbackCreateViewModel { get; set; }
        public Feedback TestFeedback { get; set; }
        public CollateParameters TestParameters { get; set; }

        private IEnumerable<Feedback> GenerateFeedbacks()
        {
            return new List<Feedback>
            {
                new Feedback
                {
                    Id = 1,
                    Email = "farefg22@gmail.com",
                    ServiceRate = 2,
                    PriceRate = 3,
                    SupportRate = 4,
                    Suggestions = "Please be more careful with animals!",
                    UserId = 4
                },
                new Feedback
                {
                    Id = 1,
                    Email = "yaayd80@gmail.com",
                    ServiceRate = 4,
                    PriceRate = 3,
                    SupportRate = 4,
                    UserId = 6
                }
            };
        }

        private IEnumerable<FeedbackReadViewModel> GenerateFeedbacksViewModel()
        {
            return new List<FeedbackReadViewModel>
            {
                new FeedbackReadViewModel
                {
                    Email = "farefg22@gmail.com",
                    ServiceRate = 2,
                    PriceRate = 3,
                    SupportRate = 4,
                    Suggestions = "Please be more careful with animals!"
                },
                new FeedbackReadViewModel
                {
                    Email = "yaayd80@gmail.com",
                    ServiceRate = 4,
                    PriceRate = 3,
                    SupportRate = 4
                }
            };
        }

        private FeedbackCreateViewModel GenerateCreateFeedbackViewModel()
        {
            return new FeedbackCreateViewModel
            {
                Email = "farefg22@gmail.com",
                ServiceRate = 3,
                PriceRate = 3,
                SupportRate = 4,
                Suggestions = "Please be more careful with animals!",
                UserId = 4
            };
        }

        private Feedback GenerateFeedback()
        {
            return new Feedback
            {
                Email = "farefg22@gmail.com",
                ServiceRate = 3,
                PriceRate = 3,
                SupportRate = 4,
                Suggestions = "Please be more careful with animals!",
                UserId = 4
            };
        }


        private CollateParameters GenerateParameters()
        {
            return new CollateParameters
            {
                FilterParam = null,
                OrderByParam = null,
                TakeCount = 20,
                SkipCount = 0
            };
        }
    }
}
