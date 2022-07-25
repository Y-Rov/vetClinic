using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;

namespace Application.Test.Fixtures
{
    public class FeedbackServiceFixture
    {
        public FeedbackServiceFixture()
        {
            var fixture =
                new Fixture().Customize(new AutoMoqCustomization());

            ExpectedFeedbacks = GeneratePagedList();
            ExpectedEmptyFeedbacks = GenerateEmptyPagedList();
            TestFeedback = GenerateFeedback();
            TestParameters = GenerateFeedbackParameters();

            MockRepository = fixture.Freeze<Mock<IFeedbackRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

            MockService = new FeedbackService(
                MockRepository.Object,
                MockLogger.Object);
        }

        public FeedbackService MockService { get; }
        public Mock<IFeedbackRepository> MockRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }

        public PagedList<Feedback> ExpectedFeedbacks { get; set; }
        public PagedList<Feedback> ExpectedEmptyFeedbacks { get; set; }
        public Feedback TestFeedback { get; set; }
        public FeedbackParameters TestParameters { get; set; }
        
        private IList<Feedback> GenerateFeedbacks()
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

        private Feedback GenerateFeedback()
        {
            return new Feedback
            {
                Id = 1,
                Email = "farefg22@gmail.com",
                ServiceRate = 3,
                PriceRate = 3,
                SupportRate = 4,
                Suggestions = "Please be more careful with animals!",
                UserId = 4
            };
        }

        private PagedList<Feedback> GeneratePagedList()
        {
            List<Feedback> feedbacks = GenerateFeedbacks().ToList();
            return new PagedList<Feedback>(feedbacks,feedbacks.Count, 1,5);
        }

        public PagedList<Feedback> GenerateEmptyPagedList()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            return new PagedList<Feedback>(feedbacks, feedbacks.Count, 1, 5);
        }

        private FeedbackParameters GenerateFeedbackParameters()
        {
            return new FeedbackParameters()
            {
                PageSize = 5,
                PageNumber = 1
            };
        }
    }
}
 