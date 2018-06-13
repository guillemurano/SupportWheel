using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SupportWheel.Api.Repositories;
using SupportWheel.Api.Services;
using SupportWheel.Tests.Stubs;
using Xunit;

namespace SupportWheel.Tests.Services 
{
    public class SchedulerServiceTest
    {

        private readonly int QUANTITY = 10;

        [Fact]
        public void GivenADateInTheFuture_WhenCallGenerateMethod_ThenReturnTwoShiftForEachDayTillFutureDate()
        {
            //Given
            var dateTo = DateTime.Today.AddDays(5);
            var shifts = ShiftStub.CreateToDate(dateTo);

            var repository = new Mock<IShiftRepository>();
            repository.Setup(r => r.Get(null, null))
                .Returns(shifts);

            var service = new SchedulerService(repository.Object);

            //When
            var results = service.Generate(dateTo);
            
            //Then
            results.Should().BeEquivalentTo(shifts);
            repository.Verify(r => r.Get(null, null), Times.Exactly(1));
        }

        [Fact]
        public void GivenNoDate_WhenCallGenerateMethod_ThenReturnTwoShiftsForToday()
        {
            //Given
            DateTime? dateTo = null;
            var shifts = ShiftStub.CreateToDate(dateTo);

            var repository = new Mock<IShiftRepository>();
            repository.Setup(r => r.Get(null, null))
                .Returns(shifts);

            var service = new SchedulerService(repository.Object);

            //When
            var results = service.Generate(dateTo);
            
            //Then
            results.Should().BeEquivalentTo(shifts);
            results.Should().OnlyContain(s => s.Date == DateTime.Today);
            repository.Verify(r => r.Get(null, null), Times.Exactly(1));
        }

        [Fact]
        public void GivenAListOfShiftsWithDirtyTrue_WhenCallSaveShiftsWithApproveTrue_ThenAllShiftsDirtyAreChangedToFalse()
        {
            //Given
            var shifts = ShiftStub.CreateWithDirtyStatus(QUANTITY, true);
            var approve = true;

            var repository = new Mock<IShiftRepository>();
            repository.Setup(r => r.Get(null, null))
                .Returns(shifts);

            var service = new SchedulerService(repository.Object);

            //When
            service.SaveShifts(approve);
            var results = service.GetAll();

            //Then
            Assert.True(results.All(s => s.IsDirty == false));
        }
    }
}