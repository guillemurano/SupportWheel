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
    public class EngineerRepository : IRepository<Engineer>
    {
        protected readonly DbContext Context;
        protected readonly DbSet<Engineer> Set;

        public EngineerRepository()
        {
            this.Context = new SupportWheelContext();
            this.Set = this.Context.Set<Engineer>();
        }

        public virtual IQueryable<Engineer> GetQueryable()
        {
            return Context.Set<Engineer>().AsQueryable();
        }

        public virtual IList<Engineer> Get(
            Expression<Func<Engineer, bool>> filter = null,
            Func<IQueryable<Engineer>, IOrderedQueryable<Engineer>> orderBy = null,
            params Expression<Func<Engineer, object>>[] includes)
        {
            return GetQuery(filter, orderBy, includes).ToList();
        }

        public PagedQueryResult<Engineer> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<Engineer, bool>> filter = null,
            Func<IQueryable<Engineer>, IOrderedQueryable<Engineer>> orderBy = null,
            params Expression<Func<Engineer, object>>[] includes)
        {
            IQueryable<Engineer> query = this.GetQuery(filter, orderBy, includes);

            long count = query.LongCount();
            int totalPages = (int)Math.Ceiling((0D + count) / pageSize);

            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return new PagedQueryResult<Engineer>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = count,
                TotalPages = totalPages,

                Items = query.ToList()
            };
        }

        protected virtual IQueryable<Engineer> GetQuery(
            Expression<Func<Engineer, bool>> filter = null,
            Func<IQueryable<Engineer>, IOrderedQueryable<Engineer>> orderBy = null,
            params Expression<Func<Engineer, object>>[] includes)
        {
            IQueryable<Engineer> query = this.Set;

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

        public virtual Engineer GetById(params object[] keyValues)
        {
            return this.Set.Find(keyValues);
        }

        public virtual void Insert(Engineer Engineer)
        {
            this.Set.Add(Engineer);
        }

        public virtual void Update(Engineer Engineer)
        {
            var e = this.Set.Find(Engineer.Id);
            e.Name = Engineer.Name;
            e.Surname = Engineer.Surname;

            SaveChanges();
        }

        public virtual void Delete(params object[] keyValues)
        {
            var Engineer = this.Set.Find(keyValues);

            if (Engineer != null)
            {
                this.Delete(Engineer);
            }
        }

        public virtual void Delete(Engineer Engineer)
        {
            this.Set.Remove(Engineer);
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