using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures
{
    public class FeedbackServiceFixture
    {
        public FeedbackServiceFixture()
        {
            var fixture =
                new Fixture().Customize(new AutoMoqCustomization());

            ExpectedFeedbacks = GenerateFeedbacks();
            TestFeedback = GenerateFeedback();

            MockRepository = fixture.Freeze<Mock<IFeedbackRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

            MockService = new FeedbackService(
                MockRepository.Object,
                MockLogger.Object);
        }

        public FeedbackService MockService { get; }
        public Mock<IFeedbackRepository> MockRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }

        public IList<Feedback> ExpectedFeedbacks { get; set; }
        public Feedback TestFeedback { get; set; }

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
    }
}
 