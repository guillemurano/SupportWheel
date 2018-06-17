using FluentAssertions;
using Moq;
using SupportWheel.Api.Models;
using SupportWheel.Api.Repositories;
using SupportWheel.Api.Services;
using SupportWheel.Tests.Stubs;
using Xunit;

namespace SupportWheel.Tests.Services
{
    public class EngineerServiceTest
    {
        private readonly int QUANTITY = 10;

        [Fact]
        public void GivenEngineerService_WhenGetAllIsInvoked_ThenShouldReturnAllEngineersInRepository()
        {
            //Given
            var engineers = EngineerStub.CreateMultiple(QUANTITY);

            var repository = new Mock<IRepository<Engineer>>();
            repository.Setup(r => r.Get(null, null))
                .Returns(engineers);

            var service = new EngineerService(repository.Object);

            //When
            var results = service.GetAll();

            //Then
            results.Should().BeEquivalentTo(engineers);
            repository.Verify(r => r.Get(null, null), Times.Exactly(1));
        }

    }
}