using System;
using System.Linq.Expressions;
using SupportWheel.Api.Models;

namespace SupportWheel.Api.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {
        void SaveAll(Expression<Func<Shift, bool>> filter);
        void DeleteAll(Expression<Func<Shift, bool>> filter);

    }
}