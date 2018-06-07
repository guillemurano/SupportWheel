using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using SupportWheel.Api.Models;

namespace SupportWheel.Tests.Stubs
{
    public static class EngineerStub
    {
        public static Engineer Create(long id)
        {
            return Builder<Engineer>.CreateNew()
                .With(d => d.Id = id)
                .Build();
        }

        public static List<Engineer> CreateMultiple(int quantity)
        {
            var id = Guid.NewGuid();
            var deals = new List<Engineer>();

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException("The quantity should be at least one.");
            }

            deals = Builder<Engineer>
                .CreateListOfSize(quantity)
                .Build() as List<Engineer>;

            return deals;
        }
    }
}