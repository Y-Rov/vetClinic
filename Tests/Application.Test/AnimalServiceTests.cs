using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test
{
    public class AnimalServiceTests
    {
        public AnimalServiceTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAnimalRepository = fixture.Freeze<Mock<IAnimalRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockAnimalService = new AnimalService(
                MockAnimalRepository.Object,
                MockLoggerManager.Object);
        }

        public Mock<IAnimalRepository> MockAnimalRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public AnimalService MockAnimalService { get; }
    }
}
