using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using SupportWheel.Api.Models;

namespace SupportWheel.Tests.Stubs
{
    public static class ShiftStub
    {
        public static Shift Create(long id)
        {
            return Builder<Shift>.CreateNew()
                .With(d => d.Id = id)
                .Build();
        }

        public static List<Shift> CreateToDate(DateTime? date)
        {
            var quantity = date.HasValue ? 
                (int)(date.Value - DateTime.Today).TotalDays : 1;
                
            var shifts = new List<Shift>();

            shifts = Builder<Shift>
                .CreateListOfSize(quantity)
                .All().With(s => s.IsDirty = true)
                .With(s => s.Turn = new Random().Next(1,2))
                .Build() as List<Shift>;

            return shifts;
        }

        public static List<Shift> CreateWithDirtyStatus(int quantity, bool isDirty)
        {
            var shifts = new List<Shift>();

            shifts = Builder<Shift>
                .CreateListOfSize(quantity)
                .All().With(s => s.IsDirty = isDirty)
                .With(s => s.Turn = new Random().Next(1,2))
                .Build() as List<Shift>;

            return shifts;
        }
    }
}