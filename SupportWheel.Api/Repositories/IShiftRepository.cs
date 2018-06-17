using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SupportWheel.Api.Models;

namespace SupportWheel.Api.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {
        void AcceptAll(Expression<Func<Shift, bool>> filter);
        void DeleteAll(Expression<Func<Shift, bool>> filter);
        bool Insert(IList<Shift> shifts);
        IEnumerable<Engineer> GetAvailableEngineers(Expression<Func<Engineer, bool>> filter);
    }
}