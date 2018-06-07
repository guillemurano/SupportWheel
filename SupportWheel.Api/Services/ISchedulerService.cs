using System;
using System.Collections.Generic;
using SupportWheel.Api.Generics;

namespace SupportWheel.Api.Services
{
    public interface ISchedulerService<TEntity> where TEntity : class
    {
        IList<TEntity> Get(DateTime from, DateTime? to);

        IList<TEntity> Generate(DateTime from, DateTime? to);
        void SaveShifts(bool approve);
    }
}