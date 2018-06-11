using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using SupportWheel.Api.Generics;
using SupportWheel.Api.Models;

namespace SupportWheel.Api.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        protected readonly DbContext Context;
        protected readonly DbSet<Shift> Set;

        public ShiftRepository()
        {
            this.Context = new SupportWheelContext();
            this.Set = this.Context.Set<Shift>();
        }

        public virtual IQueryable<Shift> GetQueryable()
        {
            return Context.Set<Shift>().AsQueryable();
        }

        public virtual IList<Shift> Get(
            Expression<Func<Shift, bool>> filter = null,
            Func<IQueryable<Shift>, IOrderedQueryable<Shift>> orderBy = null,
            params Expression<Func<Shift, object>>[] includes)
        {
            return GetQuery(filter, orderBy, includes).ToList();
        }

        public PagedQueryResult<Shift> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<Shift, bool>> filter = null,
            Func<IQueryable<Shift>, IOrderedQueryable<Shift>> orderBy = null,
            params Expression<Func<Shift, object>>[] includes)
        {
            IQueryable<Shift> query = this.GetQuery(filter, orderBy, includes);

            long count = query.LongCount();
            int totalPages = (int)Math.Ceiling((0D + count) / pageSize);

            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return new PagedQueryResult<Shift>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = count,
                TotalPages = totalPages,

                Items = query.ToList()
            };
        }

        protected virtual IQueryable<Shift> GetQuery(
            Expression<Func<Shift, bool>> filter = null,
            Func<IQueryable<Shift>, IOrderedQueryable<Shift>> orderBy = null,
            params Expression<Func<Shift, object>>[] includes)
        {
            IQueryable<Shift> query = this.Set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual Shift GetById(params object[] keyValues)
        {
            return this.Set.Find(keyValues);
        }

        public virtual void Insert(Shift Shift)
        {
            this.Set.Add(Shift);
        }

        public virtual void Update(Shift Shift)
        {            
            throw new NotImplementedException();
        }

        public virtual void SaveAll(Expression<Func<Shift, bool>> filter)
        {
            var shifts = this.Get(filter).Select(s => new Shift() {
                Id = s.Id, 
                Engineer = s.Engineer, 
                Date = s.Date, 
                Turn = s.Turn, 
                IsDirty = false                
            });

            this.Set.UpdateRange(shifts);
        }

        public virtual void Delete(params object[] keyValues)
        {
            var Shift = this.Set.Find(keyValues);

            if (Shift != null)
            {
                this.Delete(Shift);
            }
        }

        public virtual void Delete(Shift Shift)
        {
            this.Set.Remove(Shift);
        }

        public virtual void DeleteAll(Expression<Func<Shift, bool>> filter)
        {
            this.Set.RemoveRange(this.Get(filter));
            SaveChanges();
        }

        public void SaveChangesWithScope(TransactionScope scope)
        {
            using (scope)
            {
                this.Context.SaveChanges();
            }
        }

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}