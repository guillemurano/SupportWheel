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
                (int)(DateTime.Today - date.Value).TotalDays : 1;

            return CreateWithDirtyStatus(quantity, true);
        }

        public static List<Shift> CreateWithDirtyStatus(int quantity, bool isDirty)
        {
            var id = Guid.NewGuid();
            var shifts = new List<Shift>();

            shifts = Builder<Shift>
                .CreateListOfSize(quantity)
                .All().With(s => s.IsDirty = isDirty)
                .Build() as List<Shift>;

            return shifts;
        }
    }
}