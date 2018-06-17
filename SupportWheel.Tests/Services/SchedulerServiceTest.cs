using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SupportWheel.Api.Models;
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
            var availableEngineers = shifts.Select(s => new Engineer() {
                    Id = s.EngineerId,
                    Shifts = shifts.Where(sh => sh.EngineerId == s.EngineerId).ToList()
                }).ToList();

            var repository = new Mock<IShiftRepository>();

            repository.Setup(r => r.GetAvailableEngineers(It.IsAny<Expression<Func<Engineer, bool>>>()))
                .Returns(availableEngineers);

            repository.Setup(r => r.Get(It.IsAny<Expression<Func<Shift, bool>>>(),null))
                .Returns(shifts);

            repository.Setup(r => r.Insert(shifts))
                .Returns(true);

            var service = new SchedulerService(repository.Object);

            //When
            var results = service.Generate(dateTo).ToList();
            
            //Then
            results.TrueForAll(s => s.IsDirty);
            results.Should().Contain(s => availableEngineers.Any(e => e.Id == s.EngineerId));
        }

        [Fact]
        public void GivenNoDate_WhenCallGenerateMethod_ThenReturnTwoShiftsForToday()
        {
            //Given
            DateTime? dateTo = null;
            var shifts = ShiftStub.CreateToDate(dateTo);
            var availableEngineers = shifts.Select(s => new Engineer() {
                    Id = s.EngineerId,
                    Shifts = shifts.Where(sh => sh.EngineerId == s.EngineerId).ToList()
                }).ToList();

            var repository = new Mock<IShiftRepository>();

            repository.Setup(r => r.GetAvailableEngineers(It.IsAny<Expression<Func<Engineer, bool>>>()))
                .Returns(availableEngineers);

            repository.Setup(r => r.Get(It.IsAny<Expression<Func<Shift, bool>>>(),null))
                .Returns(shifts);

            repository.Setup(r => r.Insert(shifts))
                .Returns(true);

            var service = new SchedulerService(repository.Object);

            //When
            var results = service.Generate(dateTo).ToList();
            
            //Then
            results.TrueForAll(s => s.IsDirty);
            results.Should().Contain(s => availableEngineers.Any(e => e.Id == s.EngineerId));
        }

        [Fact]
        public void GivenAListOfShiftsWithDirtyTrue_WhenCallSaveShiftsWithApproveTrue_ThenAllShiftsDirtyAreChangedToFalse()
        {
            //Given
            var shifts = ShiftStub.CreateWithDirtyStatus(QUANTITY, true);
            var approve = true;

            var repository = new Mock<IShiftRepository>();
            repository.Setup(r => r.AcceptAll(It.IsAny<Expression<Func<Shift, bool>>>()))
                .Callback(() => shifts.Select(s => { s.IsDirty = false; return s; }).ToList())
                .Verifiable();

            var service = new SchedulerService(repository.Object);

            //When
            service.SaveShifts(approve);
            var results = service.GetAll();

            //Then
            shifts.TrueForAll(s => !s.IsDirty);
        }

        [Fact]
        public void GivenAListOfShiftsWithDirtyTrue_WhenCallSaveShiftsWithApproveFalse_ThenAllShiftsDirtyAreDeleted()
        {
            //Given
            var shifts = ShiftStub.CreateWithDirtyStatus(QUANTITY, true);
            var approve = false;

            var repository = new Mock<IShiftRepository>();
            repository.Setup(r => r.DeleteAll(It.IsAny<Expression<Func<Shift, bool>>>()))
                .Callback(() => shifts = null).Verifiable();

            var service = new SchedulerService(repository.Object);

            //When
            service.SaveShifts(approve);
            var results = service.GetAll();

            //Then
            shifts.Should().BeNullOrEmpty();
        }
    }
}