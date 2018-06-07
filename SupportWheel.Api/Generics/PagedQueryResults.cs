using System.Collections.Generic;

namespace SupportWheel.Api.Generics{    
    public class PagedQueryResult<TEntity>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public long TotalItems { get; set; }
        public IList<TEntity> Items { get; set; }
    }
}