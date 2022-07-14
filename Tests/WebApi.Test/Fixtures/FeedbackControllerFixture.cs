using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
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

            ExpectedFeedbacks = GeneratePagedList();
            ExpectedFeedbacksViewModel = GeneratePagedReadList();
            TestFeedbackCreateViewModel = GenerateCreateFeedbackViewModel();
            EmptyFeedbacks = GenerateEmptyPagedList();
            EmptyReadFeedbacks = GenerateEmptyReadPagedList();
            TestParameters = GenerateParameters();
            TestFeedback = GenerateFeedback();

            MockFeedbackService = fixture.Freeze<Mock<IFeedbackService>>();
            MockCreateFeedbackMapper = fixture.Freeze<Mock<IViewModelMapper<FeedbackCreateViewModel, Feedback>>>();
            MockPagedMapper = fixture.Freeze<Mock<IViewModelMapper<PagedList<Feedback>, PagedReadViewModel<FeedbackReadViewModel>>>>();

            MockController = new FeedbackController(
                MockFeedbackService.Object,
                MockCreateFeedbackMapper.Object,
                MockPagedMapper.Object);
        }

        public FeedbackController MockController { get; }
        public Mock<IFeedbackService> MockFeedbackService { get; }
        public Mock<IViewModelMapper<FeedbackCreateViewModel, Feedback>> MockCreateFeedbackMapper { get; }
        public Mock<IViewModelMapper<PagedList<Feedback>, PagedReadViewModel<FeedbackReadViewModel>>> MockPagedMapper { get; }

        public PagedList<Feedback> ExpectedFeedbacks { get; set; }
        public PagedReadViewModel<FeedbackReadViewModel> ExpectedFeedbacksViewModel { get; set; }
        public PagedList<Feedback> EmptyFeedbacks { get; set; }
        public PagedReadViewModel<FeedbackReadViewModel> EmptyReadFeedbacks { get; set; }

        public FeedbackCreateViewModel TestFeedbackCreateViewModel { get; set; }
        public Feedback TestFeedback { get; set; }
        public FeedbackParameters TestParameters { get; set; }

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

        public PagedList<Feedback> GeneratePagedList()
        {
            List<Feedback> feedbacks = GenerateFeedbacks().ToList();
            return new PagedList<Feedback>(feedbacks, feedbacks.Count, 1, 4);
        }

        public PagedReadViewModel<FeedbackReadViewModel> GeneratePagedReadList()
        {
            List<FeedbackReadViewModel> feedbacks = GenerateFeedbacksViewModel().ToList();
            return new PagedReadViewModel<FeedbackReadViewModel>
            {
                CurrentPage = 1,
                TotalPages = 10,
                PageSize = 4,
                TotalCount = feedbacks.Count,
                HasPrevious = false,
                HasNext = true,
                Entities = feedbacks
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

        public PagedList<Feedback> GenerateEmptyPagedList()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            return new PagedList<Feedback>(feedbacks, feedbacks.Count, 1, 4);
        }

        public PagedReadViewModel<FeedbackReadViewModel> GenerateEmptyReadPagedList()
        {
            List<FeedbackReadViewModel> feedbacks = new List<FeedbackReadViewModel>();
            return new PagedReadViewModel<FeedbackReadViewModel>()
            {
                Entities = feedbacks,
                TotalCount = feedbacks.Count,
                CurrentPage = 1,
                TotalPages = 10,
                PageSize = 4,
                HasPrevious = false,
                HasNext = true
            };
        }

        private FeedbackParameters GenerateParameters()
        {
            return new FeedbackParameters()
            {
                PageSize = 4,
                PageNumber = 1
            };
        }
    }
}
